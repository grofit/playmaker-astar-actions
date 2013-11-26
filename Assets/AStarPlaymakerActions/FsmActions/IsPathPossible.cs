using Pathfinding;

namespace HutongGames.PlayMaker.Pathfinding
{
   [ActionCategory("A Star")]
   [Tooltip("This is a cheap way to check whether a path from one vector3 position to another is possible, without walking over unwalkable nodes. It gets broken if the actual target node is unwalkable and will then only return false until you update the graph again. So watch out :) ")]
   public class IsPathPossible : FsmStateAction 
   {
        [Tooltip("Start of the path")]
        public FsmVector3 Start;

        [Tooltip("End pf the path")]
        public FsmVector3 End;
	  
        public FsmBool PathIsPossible;
        public FsmBool everyFrame;

        private Node startNode, endNode;
   
		public override void OnEnter() 
        {
			startNode = AstarPath.active.GetNearest (Start.Value).node;
			endNode = AstarPath.active.GetNearest (End.Value).node;
			PathIsPossible.Value = PathUtilities.IsPathPossible(startNode,endNode);	
			if (!everyFrame.Value) { Finish(); }
		} 
		
		public override void OnUpdate() 
        {
			PathIsPossible.Value = PathUtilities.IsPathPossible(startNode,endNode);	
		} 
	}
}