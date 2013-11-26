using HutongGames.PlayMaker.Behaviours;
using UnityEngine;
using Pathfinding;
using Pathfinding.Nodes;
using System.Collections.Generic;
using System.Linq;
using FsmPathfinding;

namespace HutongGames.PlayMaker.Pathfinding
{
   [ActionCategory("A Star")]
   [Tooltip("This action gets you either all the nodes within a gameObjects collider or bounds, or it gets you all walkable ones in that area. Depending on the graph size it can be incredibly expensive. It only works on one graph at once for now (only the graph closest to the center of the local game object)")]
   public class GetNodesInArea : FsmStateAction 
   {
		[ActionSection("Input")]
		[Tooltip("The GameObject from which to take the coordinates")]		
		public FsmOwnerDefault localGameObject;
		
		[Tooltip("Check this to only get walkable nodes. This is much much cheaper than getting all of them. Even if you get all walkable nodes of the entire graph.")]
		public FsmBool getWalkableOnly;
		
		[Tooltip("Turn this off to use the render bounds of your gameObject instead. I almost died laughing when I figured out that using this on anything but the largest gameObjects will result in a much better performance than just letting unity check whether the node-position is within the bounds (a cube^^). Something's definitely amiss there :D ")]
		public FsmBool useRealCollider;

		[ActionSection("Output")]
		[RequiredField]
		[ObjectType(typeof(FsmNodes))]
		[Tooltip("All the nodes within the area")]
		public FsmObject nodesOutput;
		
		private Node node;
		private List<Node> nodes;
		private int index;
		private Bounds bounds;
		private GameObject targetGameObject;
		
		public override void Reset() 
        {
			useRealCollider = true;
			getWalkableOnly = false;
		}
   
		public override void OnEnter() 
        {
			targetGameObject = localGameObject.OwnerOption == OwnerDefaultOption.UseOwner ? Owner : localGameObject.GameObject.Value; // get the object that we want to use for the check
			bounds = targetGameObject.renderer.bounds; // get object's bounds
			node = AstarPath.active.GetNearest(targetGameObject.transform.position).node; // get one node to start with Eg. for getting the graph type or for flooding it.
			
			if(node.GetType() == typeof(GridNode) )
			{
				Debug.Log("It's a grid node!");
				UpdateNodesInGridGraph();
			}
			else 
			{
				UpdateNodesInArea();
				nodesOutput.Value = new FsmNodes {Value = nodes};
			}

			Debug.Log("Count in Area :" + (nodesOutput.Value as FsmNodes).Value.Count);
			Finish();
        } 
		
		void UpdateNodesInGridGraph() // this is the optimized version. Other graphs cannot be optimised this way because they have irregular structures unfortunately. Still, we shouldn't miss on the advantages of this specific graph type, so I included it specifically.
		{
			nodes = new List<Node> {node};
		    var graph = AstarPath.active.graphs[node.graphIndex] as GridGraph;
			
			var graphUpdateShape = new GraphUpdateShape(); 
			if (useRealCollider.Value)
			{ GetNodesFromCollider(graphUpdateShape); }
			else
			{
				bounds = (targetGameObject.renderer.GetComponent<MeshFilter>()).sharedMesh.bounds;
				RecalculateBox(graphUpdateShape);
			}
			
			nodes = graph.GetNodesInArea(graphUpdateShape);
			
			if(getWalkableOnly.Value)
			{
				var connected = new List<Node>();
				for(var i=0; i < nodes.Count(); i++ )
				{
					if(nodes[i].walkable)
					{ connected.Add(nodes[i]); }
				}
				nodesOutput.Value = new FsmNodes {Value = connected};
			}
			else
			{ nodesOutput.Value = new FsmNodes { Value = nodes }; }

			Debug.Log("i" + nodes.Count);
		}

       private void GetNodesFromCollider(GraphUpdateShape graphUpdateShape)
       {
           if (targetGameObject.collider is BoxCollider) // take render bounds, then turn them into world coordinates
           {
               Debug.Log("It's a box collider");
               var castCollider = (targetGameObject.collider as BoxCollider);
               bounds.center = castCollider.center;
               bounds.size = castCollider.size;
               RecalculateBox(graphUpdateShape);
           }
           else if (targetGameObject.collider is MeshCollider)
           {
               Debug.Log("It's a mesh collider!");
               var castCollider = (targetGameObject.collider as MeshCollider);
               graphUpdateShape.points = castCollider.sharedMesh.vertices;
               for (var i = 0; i < graphUpdateShape.points.Count(); i++)
               { graphUpdateShape.points[i] = targetGameObject.transform.TransformPoint(castCollider.sharedMesh.vertices[i]); }
           }
           else
           {
               Debug.Log("This type of collider is not specifically supported. Using bounds instead...");
               RecalculateBox(graphUpdateShape);
           }
       }

       void RecalculateBox(GraphUpdateShape graphUpdateShape) 
       {
			graphUpdateShape.points = new Vector3[8];
			graphUpdateShape.points[0] = targetGameObject.transform.TransformPoint(new Vector3(bounds.min.x, bounds.min.y, bounds.max.z));
			graphUpdateShape.points[1] = targetGameObject.transform.TransformPoint(new Vector3(bounds.max.x, bounds.min.y, bounds.max.z));
			graphUpdateShape.points[2] = targetGameObject.transform.TransformPoint(new Vector3(bounds.max.x, bounds.min.y, bounds.min.z));
			graphUpdateShape.points[3] = targetGameObject.transform.TransformPoint(new Vector3(bounds.min.x, bounds.min.y, bounds.min.z));
			graphUpdateShape.points[7] = targetGameObject.transform.TransformPoint(new Vector3(bounds.min.x, bounds.max.y, bounds.min.z));
			graphUpdateShape.points[4] = targetGameObject.transform.TransformPoint(new Vector3(bounds.min.x, bounds.max.y, bounds.max.z));
			graphUpdateShape.points[5] = targetGameObject.transform.TransformPoint(new Vector3(bounds.max.x, bounds.max.y, bounds.max.z));
			graphUpdateShape.points[6] = targetGameObject.transform.TransformPoint(new Vector3(bounds.max.x, bounds.max.y, bounds.min.z));		
		}
		
		void UpdateNodesInArea() 
        {
			nodes = new List<Node> {node};
		    if(getWalkableOnly.Value) // actually, this does not get all walkables in the area, it just floods outwards along the walkables from the center of the gameObject. anyways, could be useful, so I left it in.
			{
				for(var i=0; i < nodes.Count; i++) 
				{ nodes[i].GetConnections(CheckNode); }
			}
			else
			{
				var allNodes = AstarPath.active.graphs[node.graphIndex].nodes; // get all nodes, no .ToList(); to save performance (save wherever you can :D )
				foreach (var currentNode in allNodes)
				{ CheckNode(currentNode); }
			}
		}
		
		public void CheckNode(Node nodeToCheck)
		{
		    var nodePosition = new Vector3(nodeToCheck.position.x,nodeToCheck.position.y,nodeToCheck.position.z);
		    var normalisedNodePosition = nodePosition*Int3.PrecisionFactor;
			if(useRealCollider.Value) 
			{
                if ((!nodes.Contains(nodeToCheck)) && normalisedNodePosition.IsInside(targetGameObject.collider)) // check if the node in question is both in the collider and NOT in the list already.
				{ nodes.Add(nodeToCheck); }			
			}
			else
			{
                if ((!nodes.Contains(nodeToCheck)) && bounds.Contains(normalisedNodePosition)) // check if the node in question is both in the bounds and NOT in the list already.
				{ nodes.Add(nodeToCheck); }
			}
		}
	}
}