using UnityEngine;
using Pathfinding;

namespace HutongGames.PlayMaker.Pathfinding
{
   [ActionCategory("A Star")]
   [Tooltip("This action finds out if, after using the updateGraph action (or similar), there will or will not be a possible path between the start position and the end position. It does not actually update the graph.")]
   public class WillPathBePossible   : FsmStateAction 
   {
        [RequiredField]
        [Tooltip("Object that needs to be added to the graph")]
        [CheckForComponent(typeof(Collider))]
        public FsmOwnerDefault target;    
	  
        [Tooltip("Start of the path")]
        public FsmVector3 Start;

        [Tooltip("End of the path")]
        public FsmVector3 End;

        [Tooltip("Indicates if a path is possible")]
        public FsmBool PathIsPossible;
	  
        public FsmBool everyFrame;
   
		public void CheckIfPathIsPossible() 
        {
			var targetGraphUpdate = new GraphUpdateObject(Fsm.GetOwnerDefaultTarget(target).collider.bounds);
			var startNode = AstarPath.active.GetNearest (Start.Value).node;
			var endNode = AstarPath.active.GetNearest (End.Value).node;
			PathIsPossible.Value = GraphUpdateUtilities.UpdateGraphsNoBlock(targetGraphUpdate, startNode, endNode, true);
		}
   
		public override void OnEnter() 
        {
			CheckIfPathIsPossible();
			if (!everyFrame.Value) { Finish(); }
		} 

		public override  void OnUpdate() 
        {
			CheckIfPathIsPossible();
			if (!everyFrame.Value) { Finish(); }
		}
	}
}