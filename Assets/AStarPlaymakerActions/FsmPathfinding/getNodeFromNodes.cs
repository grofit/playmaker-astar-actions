using System;
using HutongGames.PlayMaker;
using FsmPathfinding;
using Pathfinding;
using System.Linq;
using HutongGames.PlayMaker.Helpers;
using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Pathfinding
{
	[ActionCategory("A Star")]
	[Tooltip("Currently supports both FsmNodes and FsmGridNodes. More to come. Gets a node at a certain index from an array/list of nodes")]
	public class getNodeFromNodes : HutongGames.PlayMaker.FsmStateAction
	{
		[ActionSection("Input")]
		[RequiredField]
		[ObjectType(typeof(FsmNodes))]
		[Tooltip("The nodes ")	]
		public FsmObject nodes;
		
		[RequiredField]
		[Tooltip("Index of the node")	]
		public FsmInt index ;
		
		[ActionSection("Output")]
		[ObjectType(typeof(FsmNode))]
		[Tooltip("Any type of node")	]
		public FsmObject node;

		public override void Reset()
		{
			nodes = new FsmObject();
			index = 0;
			node = new FsmObject();
		}
	  
		public override void OnEnter()  
	  	{
			var mo = nodes.Value as FsmNodes;
			
			if( mo == null || (mo.Value == null) || !node.UseVariable) 
			{
				Debug.Log("No Input");
				Finish(); 
				return;
			} 
			
			if ((mo.Value as List<Node>).Count <= index.Value)
			{
				Debug.Log("index is higher than the number of nodes in the nodes list/variable");
				Finish();
				return;
			}

			node.Value = FsmConverter.SetNode((nodes.Value as FsmNodes).Value[index.Value]);
			Finish();			
		}  
   	}
}