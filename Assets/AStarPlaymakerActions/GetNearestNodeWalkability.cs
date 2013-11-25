namespace HutongGames.PlayMaker.Actions
{
   [ActionCategory("A Star")]
   [Tooltip("This is a combination of GetClosestNode and GetNodeInfo. Since you may often need to check only the walkability of a node, this can serve as a shortcut.")]
   public class GetNearestNodeWalkability : FsmStateAction 
   {
        [RequiredField]
        [Tooltip("Aproximate position of node. It will get the closest node to this position")]
        public FsmVector3 Position;

        public FsmBool walkability;
		
		public FsmBool everyFrame;
	  
		public override void OnEnter() 
        {
			GetNearestWalkableNode();
			if (!everyFrame.Value) { Finish(); }
		} 
		
		void GetNearestWalkableNode() 
        {
			var nearest = AstarPath.active.GetNearest(Position.Value);
			walkability.Value = nearest.node.walkable;
		}
		
		public override void OnUpdate() 
        {
			GetNearestWalkableNode();
		}
	}
}