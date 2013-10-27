using System;
using HutongGames.PlayMaker;
using FsmPathfinding;
using Pathfinding;
using System.Linq;

namespace HutongGames.PlayMaker.Pathfinding
{
	[ActionCategory("A Star")]
	[Tooltip("set node attributes directly")]
	public class setNodeInfo : FsmStateAction
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

		public void Reset()
		{
			node = new FsmObject {UseVariable = true};
			penalty = new FsmInt {UseVariable = true};
			tags = new FsmInt() {UseVariable = true};
			walkable = new FsmBool {UseVariable = true};
			position = new FsmVector3 {UseVariable = true};
		}
      
		public void OnEnter() 
	  	{
			var moo = node.Value as FsmNode;
			if(moo.Value == null) 
			{
				Finish(); 
				return;
			}
			
			mohogony();
			
			if(!everyFrame.Value)
			{ Finish(); }
		}
	  
		public void OnUpdate()
		{
			mohogony();
		}
	  
		public void mohogony()
		{
			var doo = node.Value as FsmNode;
			
			if (!penalty.IsNone)
			{ doo.Value.penalty = (uint)penalty.Value; }
			
			if (!tags.IsNone)
			{ doo.Value.tags = tags.Value; }
			
			if (!walkable.IsNone) 
			{
				doo.Value.walkable = walkable.Value;
				doo.Value.UpdateNeighbourConnections ();
				doo.Value.UpdateConnections ();
			}
			
			if (!position.IsNone)
			{ doo.Value.position = new Int3((int)position.Value.x, (int)position.Value.y, (int)position.Value.z); }	  
		}
   	}
}