using HutongGames.PlayMaker.Extensions;
using UnityEngine;
using Pathfinding;
using Pathfinding.Nodes;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Pathfinding
{
   [ActionCategory("A Star")]
   [Tooltip("This action sets all nodes within a certain area that is defined by either a gameObject's bounds(outer most points forming a cube) or by it's collider. Use Update graph as often as possible, as that one is cheaper! ")]
   public class SetWalkabilityInArea : FsmStateAction 
   {
		[ActionSection("Input")]
		[Tooltip("The GameObject from which to take the coordinates")]		
		public FsmOwnerDefault localGameObject;
		
		[Tooltip("Turn this off to use the render bounds of your gameObject instead. I almost died laughing when I figured out that using this on anything but the largest gameObjects will result in a much better performance than just letting unity check whether the node-position is within the bounds (a cube^^). Something's definitely amiss there :D ")]
		public FsmBool useRealCollider;
		
		[Tooltip("Should all nodes be set to walkable, or unwalkable?")]
		public FsmBool walkability;
		
		private Node node;
		private List<Node> nodes;
		private Bounds bounds;
		private GameObject targetGameObject;
		private GridGraph graph;
   
		public override void Reset() 
        {
			useRealCollider = true;
			walkability = true;
		}
   
		public override void OnEnter() 
        {
			targetGameObject = localGameObject.OwnerOption == OwnerDefaultOption.UseOwner ? Owner : localGameObject.GameObject.Value;
			bounds = targetGameObject.renderer.bounds; // get object's bounds
			node = AstarPath.active.GetNearest(targetGameObject.transform.position).node as Node; // get one node to start with
			Debug.Log(node);
			
			if(node.GetType() == typeof(GridNode) )
			{
				Debug.Log("It's a grid node!");
				UpdateWalkabilityInGridArea();
			}
			
			else
			{
                Debug.Log("It's some other node!");
				UpdateWalkabilityInArea();
			}

			Debug.Log("Count in Area :" + nodes.Count);
			Finish();
        } 
		
		void UpdateWalkabilityInGridArea()
		{
			nodes = new List<Node>();
			nodes.Add(node);
			graph = AstarPath.active.graphs[node.graphIndex] as GridGraph;
			

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
			
			foreach(var currentNode in nodes )
			{ currentNode.walkable = walkability.Value; }
			
			Debug.Log("i" + nodes.Count);
		}	
		
		void UpdateWalkabilityInArea() 
        {
			nodes = new List<Node>();
			nodes.Add(node);
			
			var allNodes = AstarPath.active.graphs[node.graphIndex].nodes; // get all nodes, no .ToList(); to save performance (save wherever you can :D )
			foreach (var currentNode in allNodes)
			{ CheckNode(currentNode); }
            
			Debug.Log("i" + nodes.Count);
        }
		
		
		public void CheckNode(Node currentNode)
        {
            var nodePosition = new Vector3(currentNode.position.x, currentNode.position.y, currentNode.position.z);
            var normalisedNodePosition = nodePosition * Int3.PrecisionFactor;
            if (useRealCollider.Value)
            {
                if ((!nodes.Contains(currentNode)) && normalisedNodePosition.IsInside(targetGameObject.collider)) // check if the node in question is both in the collider and NOT in the list already.
                { currentNode.walkable = walkability.Value; }
            }
            else
            {
                if ((!nodes.Contains(currentNode)) && bounds.Contains(normalisedNodePosition)) // check if the node in question is both in the bounds and NOT in the list already.
                { currentNode.walkable = walkability.Value; }
            }
        }
	}
}