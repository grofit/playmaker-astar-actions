using System;
using FsmPathfinding;
using UnityEngine;

namespace HutongGames.PlayMaker.Pathfinding
{
	[ActionCategory("A Star")]
	[Tooltip("set node attributes directly")]
	public class AddNodeConnection : FsmStateAction
	{
		[ActionSection("Any type of node.")]
		[Tooltip("The node to be changed")]
		[ObjectType(typeof(FsmNode))]
		public FsmObject node;
		
		[Tooltip("The costs to traverse from one node to the other. The higher it is, the less likely the actor will use it. Useful for water or otherwise unpreferable grounds.")]
		public FsmInt cost;		

		[Tooltip("The neighbour node")]	
		[ObjectType(typeof(FsmNode))]
		public FsmObject node2;
		
		[Tooltip("Normally this action creates a one way connection. With this activated, it performs the same action on node2 too.")]
		public FsmBool pingPong;	
		
		public FsmBool everyFrame;
		
		public override void Reset()
		{
			node = new FsmObject(); 
			node2 = new FsmObject();
			cost = 1;
		}
		      
		public override void OnEnter() 
	  	{
			AddPathConnectionWithCost();
			
			if(!everyFrame.Value)
			{ Finish();	}
		}
	  
		public void AddPathConnectionWithCost()
		{
			var sourceFsmNode = node.Value as FsmNode;
            if(sourceFsmNode == null)
            { throw new NullReferenceException("Source FSM node is null"); }

			var destinationFsmNode = node2.Value as FsmNode;
            if (destinationFsmNode == null)
            { throw new NullReferenceException("Destination FSM node is null"); }

			if((sourceFsmNode.Value == null) || (destinationFsmNode.Value == null)) 
			{ 
				Debug.Log("Input Incomplete"); 
				Finish(); 
				return;
			}
			
			sourceFsmNode.Value.AddConnection(destinationFsmNode.Value, cost.Value);
 
			if (pingPong != null && pingPong.Value)
			{ destinationFsmNode.Value.AddConnection(sourceFsmNode.Value, cost.Value);	}		
		}
		
		public override void OnUpdate() 
		{ AddPathConnectionWithCost(); }
   	}
}