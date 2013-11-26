using FsmPathfinding;
using System.Linq;
using UnityEngine;

namespace HutongGames.PlayMaker.Pathfinding
{
	[ActionCategory("A Star")]
	[Tooltip(" This action returns the closest point to a gameObject on a path. It only regards world distance, not the length of the path needed to go there. So while it is cheap on performance and can get you to any path, it may sometimes take you a longer way than needed.")]
	public class GetClosestNodeOnPath : FsmStateAction
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
		
		public override void Reset() 
		{
			InputPath = null;
			DistanceFrom = null;
			node = null;
			position = null;
		} 
		
		public override void OnEnter() 
	  	{
			var underlyingFsmPath = InputPath.Value as FsmPath;
			if(underlyingFsmPath == null || underlyingFsmPath.Value == null) 
			{
				Debug.Log("Input incomplete");
				Finish(); 
				return;
			}
			
			var currentDistance = 1/0f;
			var closestNode = new FsmNode();
            var pathNodes = underlyingFsmPath.Value.vectorPath;
		    for (var i = 0; i < pathNodes.Count(); i++)
		    {
		        var o = (pathNodes[i] - Fsm.GetOwnerDefaultTarget(DistanceFrom).transform.position).sqrMagnitude;
		        if(o < currentDistance) 
		        {
		            position.Value = pathNodes[i];
                    closestNode.Value = underlyingFsmPath.Value.path[i];
		            currentDistance = o;
		        }
		    }

		    node.Value = closestNode;
			Finish();			
		}	  
   	}	
}
