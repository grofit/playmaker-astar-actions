using System;
using HutongGames.PlayMaker;
using FsmPathfinding;
using Pathfinding;
using System.Linq;
using UnityEngine;

namespace HutongGames.PlayMaker.Pathfinding
{
	[ActionCategory("A Star")]
	[Tooltip("set node attributes directly")]
	public class AddNodeConnection : HutongGames.PlayMaker.FsmStateAction
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
			Mohogony();
			
			if(!everyFrame.Value)
			{ Finish();	}
			
		}
	  
		public void Mohogony()
		{
			var mo = node.Value as FsmNode;
			var fo = node2.Value as FsmNode;
			if((mo.Value == null) || (fo.Value == null)) 
			{ 
				Debug.Log("Input Incomplete"); 
				Finish(); 
				return;
			}
			
			mo.Value.AddConnection(fo.Value as Node, cost.Value); 
			if (pingPong != null && pingPong.Value)
			{ fo.Value.AddConnection(mo.Value, cost.Value);	}		
		}
		
		public override void OnUpdate() 
		{ Mohogony();}
   	}
}