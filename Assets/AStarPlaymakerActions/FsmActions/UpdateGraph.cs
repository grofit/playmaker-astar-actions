using UnityEngine;

namespace HutongGames.PlayMaker.Pathfinding
{
   [ActionCategory("A Star")]
   [Tooltip("Update the graph once. If you want to upgrade every frame, use dynamic obstacles")]
   public class UpdateGraph : FsmStateAction 
   {
        [RequiredField]
        [Tooltip("Object that needs to be added to the graph")]
        [CheckForComponent(typeof(Collider))]
        public FsmOwnerDefault target;    
   
		public override void OnEnter() 
        {
			AstarPath.active.UpdateGraphs(Fsm.GetOwnerDefaultTarget(target).collider.bounds);
			Finish();
		} 
	}
}