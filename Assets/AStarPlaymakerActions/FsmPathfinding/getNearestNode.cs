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
	[Tooltip("Using the Seeker component on a GameObject a path is calculated and then followed.")]
	public class getNearestNode : HutongGames.PlayMaker.FsmStateAction
	{
		[ActionSection("Input")]
		[RequiredField]
		[Tooltip("Aproximate position of node. It will get the closest node to this position. Cheaply.")]
		public FsmVector3 Position;
		
		public FsmBool everyFrame;
		
		[ActionSection("Output")]
		[Tooltip("The node ")]	
		[ObjectType(typeof(FsmNode))]
		public FsmObject node;
		
		public void Reset() 
		{
			Position = null;
			everyFrame = false;
			node = null;
		}
		
		public void OnEnter() 
	  	{
			if(!node.UseVariable) 
			{
				Finish(); 
				return;
			}
			
			mohogony();
			
			var coo = node.Value as FsmNode;
			var foo = (coo as FsmNode).Value;
			
			Debug.Log(foo.area);
			
			if (!everyFrame.Value)
			{ Finish(); }			
		}
		
		public void mohogony() 
		{
			node.Value = FsmConverter.SetNode(AstarPath.active.GetNearest(Position.Value).node as Node);
			return;
		}
		
		public void OnUpdate()
		{
			mohogony();
		}
	  
   	}
}