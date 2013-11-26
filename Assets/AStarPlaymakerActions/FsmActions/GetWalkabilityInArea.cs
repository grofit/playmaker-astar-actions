using HutongGames.PlayMaker.Extensions;
using UnityEngine;
using Pathfinding;
using System.Collections.Generic;
using System.Linq;
using Pathfinding.Nodes;

namespace HutongGames.PlayMaker.Pathfinding
{
   [ActionCategory("A Star")]
   [Tooltip("This action gets you either the walkability state of all the nodes within a gameObjects collider or bounds, or it gets you the number of all walkable ones in that area. Depending on the graph size it can be incredibly expensive. It only works on one graph at once for now (only the graph closest to the center of the local game object)")]
   public class GetWalkabilityInArea : FsmStateAction 
   {
		[ActionSection("Input")]
		[Tooltip("The GameObject from which to take the coordinates")]		
		public FsmOwnerDefault localGameObject;
		
		[Tooltip("Check this to only get walkable nodes. This makes this action only return the number of walkable nodes, which can be cheaper at times.")]
		public FsmBool getWalkableOnly;
		
		[Tooltip("Turn this off to use the render bounds of your gameObject instead. I almost died laughing when I figured out that using this on anything but the largest gameObjects will result in a much better performance than just letting unity check whether the node-position is within the bounds (a cube^^). Something's definitely amiss there :D ")]
		public FsmBool useRealCollider;
		
		[ActionSection("Output")]
		[Tooltip("This returns true if all are ")]
		public FsmBool walkability;
		
		[Tooltip("The number of walkable nodes")]
		public FsmInt walkable;
		
		[Tooltip("The number of unwalkable nodes in the selection")]
		public FsmInt unwalkable;
		
		private Node node;
		private List<Node> nodes;
		private Bounds bounds;
		private GameObject targetGameObject;
   
		public override void Reset() 
        {
			useRealCollider = true;
			getWalkableOnly = false;
			unwalkable = 0;
			walkable = 0;
			walkability = true;
		}
   
		public override void OnEnter() 
        {
			walkable = 0;
			unwalkable = 0;
			walkability = true;
			targetGameObject = localGameObject.OwnerOption == OwnerDefaultOption.UseOwner ? Owner : localGameObject.GameObject.Value;
			bounds = targetGameObject.renderer.bounds;
			node = AstarPath.active.GetNearest(targetGameObject.transform.position).node;
			Debug.Log(node);
			
			if(node is GridNode)
			{
				Debug.Log("It's a grid node!");
				GetWalkabilityForGridNode();
			}
	        else
			{
                Debug.Log("It's some other node!");
				GetWalkability();
			}
			
			walkability.Value = true;
			if(unwalkable.Value != 0) 
            { walkability.Value = false; }
			
			Debug.Log("Count in Area :" + nodes.Count);
			Finish();
		}
		
		private void GetWalkabilityForGridNode() 
		{
			nodes = new List<Node> { node };
		    var graph = AstarPath.active.graphs[node.graphIndex] as GridGraph;
			
			var graphUpdateShape = new GraphUpdateShape(); 
			if (useRealCollider.Value)
			{ graphUpdateShape.RecalculatePoints(targetGameObject, bounds); }
			else
			{
				bounds = (targetGameObject.renderer.GetComponent<MeshFilter>()).sharedMesh.bounds;
                graphUpdateShape.AssignTransformedPoints(targetGameObject, bounds);
			}
			
			graphUpdateShape.convex = true;
			nodes = graph.GetNodesInArea(graphUpdateShape);
			
			if(getWalkableOnly.Value)
			{
				for(var i=0; i < nodes.Count(); i++ )
				{
					if(nodes[i].walkable)
					{ walkable.Value += 1; }
				}
			}
			else
			{
				for(var i=0; i < nodes.Count(); i++ )
				{
					if(nodes[i].walkable)
					{ walkable.Value += 1; }
					else
					{ unwalkable.Value += 1; }
				}
			}
			Debug.Log("i" + nodes.Count);
		}	
		
		private void GetWalkability() 
        {
			nodes = new List<Node>();
			nodes.Add(node);

			if(getWalkableOnly.Value)
			{
				for(var i=0; i < nodes.Count; i++) 
				{ nodes[i].GetConnections(CheckNode); }
			}
			else
			{
				var allNodes = AstarPath.active.graphs[node.graphIndex].nodes;
				foreach (var currentNode in allNodes)
				{ CheckNode(currentNode); }
			}
			
			Debug.Log("i" + nodes.Count);
		}		
		
		public void CheckNode(Node currentNode)
        {
            var nodePosition = new Vector3(currentNode.position.x, currentNode.position.y, currentNode.position.z);
            var normalisedNodePosition = nodePosition * Int3.PrecisionFactor;
            if (useRealCollider.Value)
            {
                if ((!nodes.Contains(currentNode)) && normalisedNodePosition.IsInside(targetGameObject.collider)) // check if the node in question is both in the collider and NOT in the list already.
                {
                    nodes.Add(currentNode);
                    if (currentNode.walkable)
                    { walkable.Value += 1; }
                    else
                    { unwalkable.Value += 1; }
                }
            }
            else
            {
                if ((!nodes.Contains(currentNode)) && bounds.Contains(normalisedNodePosition)) // check if the node in question is both in the bounds and NOT in the list already.
                {
                    nodes.Add(currentNode);
                    if (currentNode.walkable)
                    { walkable.Value += 1; }
                    else
                    { unwalkable.Value += 1; }
                }
            }
		}
	}
}