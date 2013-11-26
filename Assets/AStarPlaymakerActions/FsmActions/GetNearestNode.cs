using System;
using HutongGames.PlayMaker.Pathfinding;
using UnityEngine;

namespace HutongGames.PlayMaker.Pathfinding
{
	[ActionCategory("A Star")]
	[Tooltip("Using the Seeker component on a GameObject a path is calculated and then followed.")]
	public class GetNearestNode : FsmStateAction
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
		
		public override void Reset() 
		{
			Position = null;
			everyFrame = false;
			node = null;
		}
		
		public override void OnEnter() 
	  	{
			if(!node.UseVariable) 
			{
				Finish(); 
				return;
			}
			
			UpdateNearestNode();
			
			var fsmNode = node.Value as FsmNode;
            if(fsmNode == null)
            { throw new NullReferenceException("The node contains no data"); }

			var underlyingNodeValue = fsmNode.Value;
			Debug.Log(underlyingNodeValue.area);
			
			if (!everyFrame.Value)
			{ Finish(); }			
		}
		
		public void UpdateNearestNode()
		{
            var nearestNode = AstarPath.active.GetNearest(Position.Value).node;
            node.Value = new FsmNode { Value = nearestNode };
		}
		
		public override void OnUpdate()
		{
			UpdateNearestNode();
		}
   	}
}