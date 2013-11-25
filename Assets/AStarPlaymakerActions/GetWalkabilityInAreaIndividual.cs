using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
   [ActionCategory("A Star")]
   [Tooltip("For now this only works perfectly if each nodes has 8 or four or sixteen connections")]
   public class GetWalkabilityInAreaIndividual : FsmStateAction 
   {
	    [Tooltip("Center/Origin. In local space 0/0/0 is the center of the gameObject.")]
	    public FsmVector3 center;

	    [Tooltip("In local space, the size is based on the size of the gameObject. That means 1/1/1 checks every node within the gameObject. 2/2/2 checks an area twice as large")]
        public FsmVector3 size;
		
		[Tooltip("Set this to the node size as defined in the A* gameObject. If your gameObject is very very large, use higher values to save performance. Never ever use values close to 0, unless you are sure that your grid operates at that size and that your gameObject is small. )")]
		public FsmFloat nodeSize;
		
		[Tooltip("Y axis needs to always face up in local space.")]
		public Space space;
		
		[Tooltip("The GameObject from which to take the local coordinates")]		
		public FsmOwnerDefault localGameObject;
		
		public FsmEvent nextNode;
		
		public FsmEvent finished;
		
		public FsmBool walkability;
		
		public FsmVector3 pickPosition;

        [Tooltip("If this is true, it will restart the next time the action is used. This is set to true once the last node in the area is reached and the finished event is sent.")]
        public FsmBool reset;

		private Vector3 latestPos, minPos, maxPos;

        public override void Reset() 
	  	{
			nodeSize = 1f;
			size = new Vector3 (1,1,1);
			reset = true;
      	}
   
		public override void OnEnter() 
        {
			var targetGameObject = Fsm.GetOwnerDefaultTarget(localGameObject);

		    if(reset.Value)
            {
			    reset.Value = false;
			    minPos = center.Value - (size.Value/2);
			    maxPos = center.Value + (size.Value/2);
			    latestPos = minPos;
			}
			
			if (nodeSize.Value == 0.0f)
            { Finish(); }
		
			walkability.Value = true;

			if(space == Space.Self)
            { UpdateWalkabilityUsingSelf(targetGameObject); }
			else 
            { UpdateWalkability(); }
		}

       private void UpdateWalkability()
       {
           var deltaX = nodeSize.Value;
           var deltaZ = nodeSize.Value;

           while (latestPos.z < maxPos.z)
           {
               while (latestPos.x < maxPos.x)
               {
                   walkability.Value = AstarPath.active.GetNearest(latestPos).node.walkable;
                   pickPosition.Value = latestPos;
                   latestPos.x += deltaX;
                   Fsm.Event(nextNode);
                   Finish();
                   return;
               }
               latestPos.x = minPos.x;
               latestPos.z += deltaZ;
           }

           reset.Value = true;
           Fsm.Event(finished);
           Finish();
       }

       private void UpdateWalkabilityUsingSelf(GameObject targetGameObject)
       {
           var localScaleX = targetGameObject.transform.InverseTransformPoint(nodeSize.Value, 0, 0) -
                             targetGameObject.transform.InverseTransformPoint(0, 0, 0);
           var localScaleZ = targetGameObject.transform.InverseTransformPoint(0, 0, nodeSize.Value) -
                             targetGameObject.transform.InverseTransformPoint(0, 0, 0);

           var deltaX = localScaleX.magnitude;
           var deltaZ = localScaleZ.magnitude;

           while (latestPos.z < maxPos.z)
           {
               while (latestPos.x < maxPos.x)
               {
                   walkability.Value = AstarPath.active.GetNearest(targetGameObject.transform.TransformPoint(latestPos)).node.walkable;
                   pickPosition.Value = latestPos;
                   latestPos.x += deltaX;
                   Fsm.Event(nextNode);
                   Finish();
                   return;
               }
               latestPos.x = minPos.x;
               latestPos.z += deltaZ;
           }

           reset.Value = true;
           Fsm.Event(finished);
           Finish();
       }
   }
}