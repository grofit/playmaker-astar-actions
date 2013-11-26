using System;
using HutongGames.PlayMaker.Pathfinding;
using Pathfinding;

namespace HutongGames.PlayMaker.Pathfinding
{
	[ActionCategory("A Star")]
	[Tooltip("set node attributes directly")]
	public class SetNodeInfo : FsmStateAction
	{
		[ActionSection("Input")]
		[RequiredField]
		[ObjectType(typeof(FsmNode))]
		[Tooltip("The node")]	
		public FsmObject node;
		
		[Tooltip("Penlty cost for walking on this node. ")]
		public FsmInt penalty ;
		
		[Tooltip("Tags for walkability.")]
		public FsmInt tags;
		
		[Tooltip("Tags for walkability.")]	
		public FsmBool walkable;
		
		[Tooltip("node position. Change at your own risk.")]
		public FsmVector3 position;
		
		public FsmBool everyFrame;

		public override void Reset()
		{
			node = new FsmObject {UseVariable = true};
			penalty = new FsmInt {UseVariable = true};
			tags = new FsmInt() {UseVariable = true};
			walkable = new FsmBool {UseVariable = true};
			position = new FsmVector3 {UseVariable = true};
		}
      
		public override void OnEnter() 
	  	{
			var fsmNode = node.Value as FsmNode;
            if (fsmNode == null || fsmNode.Value == null) 
			{
				Finish(); 
				return;
			}
			
			SetInfoOnNode();
			
			if(!everyFrame.Value)
			{ Finish(); }
		}
	  
		public override void OnUpdate()
		{
			SetInfoOnNode();
		}
	  
		public void SetInfoOnNode()
		{
			var fsmNode = node.Value as FsmNode;
            if(fsmNode == null)
            { throw new NullReferenceException("The node is null"); }
			
			if (!penalty.IsNone)
			{ fsmNode.Value.penalty = (uint)penalty.Value; }
			
			if (!tags.IsNone)
			{ fsmNode.Value.tags = tags.Value; }
			
			if (!walkable.IsNone) 
			{
				fsmNode.Value.walkable = walkable.Value;
				fsmNode.Value.UpdateNeighbourConnections ();
				fsmNode.Value.UpdateConnections ();
			}
			
			if (!position.IsNone)
			{ fsmNode.Value.position = new Int3((int)position.Value.x, (int)position.Value.y, (int)position.Value.z); }	  
		}
   	}
}