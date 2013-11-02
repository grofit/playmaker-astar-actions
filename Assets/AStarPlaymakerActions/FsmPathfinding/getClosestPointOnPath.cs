using System;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Helpers;
using FsmPathfinding;
using Pathfinding;
using System.Linq;
using UnityEngine;

namespace HutongGames.PlayMaker.Pathfinding
{
	[ActionCategory("A Star")]
	[Tooltip(" This action returns the closest point to a gameObject on a path. It only regards world distance, not the length of the path needed to go there. So while it is cheap on performance and can get you to any path, it may sometimes take you a longer way than needed.")]
	public class getClosestNodeOnPath : HutongGames.PlayMaker.FsmStateAction
	{
		[ActionSection("Input")]
		[ObjectType(typeof(FsmPath))]
		[Tooltip("the path to check")]	
		public FsmObject InputPath;
		
		[RequiredField ]
		[Tooltip("Compare the distance of the items in the list to the position of this gameObject") ]
		public FsmOwnerDefault DistanceFrom;


		[ActionSection("Node") ]
		[ObjectType(typeof(FsmNode)) ]
		[Tooltip("closest node ")]	
		public FsmObject node;

		[ActionSection("Vector3") ]
		[Tooltip("The actual position ")]	
		public FsmVector3 position;
		
		private FsmPath goo = new FsmPath();	
		
		
		public override void Reset() 
		{
			InputPath = null;
			DistanceFrom = null;
			node = null;
			position = null;
		} 
		
		public override void OnEnter() 
	  	{
			var go = InputPath.Value as FsmPath;
			if(go == null || go.Value == null) 
			{
				Debug.Log("Input incomplete");
				Finish(); 
				return;
			} // it would continue for a frame without return
			
			var a = 1/0f;
			var doo = InputPath.Value as FsmPath;
			var coo = new FsmNode();
			
			if (doo.Value == null) 
			{ return; }			
			else 
			{
				var pathNodes = doo.Value.vectorPath;
			 	for (var i = 0; i < Enumerable.Count(pathNodes); i++)
				{
					var o = (pathNodes[i] - Fsm.GetOwnerDefaultTarget(DistanceFrom).transform.position).sqrMagnitude;
					if(o < a) 
					{
						position.Value = pathNodes[i];
						coo.Value = doo.Value.path[i];
						a = o;
					}
				}
			}
			node.Value = coo;
			Finish();			
		}	  
   	}	
}
