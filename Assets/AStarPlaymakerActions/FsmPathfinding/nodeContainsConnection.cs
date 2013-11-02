using System;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Helpers;
using FsmPathfinding;
using Pathfinding;
using System.Linq;
using AstarPathExtension;
using UnityEngine;

namespace HutongGames.PlayMaker.Pathfinding
{
	[ActionCategory("A Star")]
	[Tooltip("set node attributes directly")]
	public class nodeContainsConnection : FsmStateAction
	{

		[ActionSection("Any type of node.")]
		[ObjectType(typeof(FsmNode))]
		[Tooltip("node1")]	
		public FsmObject node;
		
		[Tooltip("node2")]
		public FsmObject node2;
		
		[ActionSection("Output")]
		public FsmBool connected;
		
		public override void Reset()
		{
			node = null; 
			node2 = null;
			connected = false;
		}		
      
		public override void OnEnter() 
	  	{
			var mo = node.Value as FsmNode;
			var fo = node2.Value as FsmNode; 
			if(mo == null || fo == null || (mo.Value == null) || (fo.Value == null)) 
			{
				Debug.Log("Input incomplete, node not valid or does not exist. Make sure you assigned it properly.");
				Finish(); 
				return;
			}
			
			var a = (node.Value as FsmNode).Value as Node;
			connected.Value = a.ContainsConnection(FsmConverter.GetAnythingShallow(node2) as Node); 
			Finish();	
		} 
   	}
}