using UnityEngine;
using System.Collections;
using Pathfinding;
using Pathfinding.Nodes;
using System.Collections.Generic;
using System.Linq;
using FsmPathfinding;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Helpers;


namespace HutongGames.PlayMaker.Actions
{
   [ActionCategory("A Star")]
   [Tooltip("This action gets you either all the nodes within a gameObjects collider or bounds, or it gets you all walkable ones in that area. Depending on the graph size it can be incredibly expensive. It only works on one graph at once for now (only the graph closest to the center of the local game object)")]
   public class GetNodesInArea   : FsmStateAction 
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
		private List<Node> connected;
		private Node[] alln;
		private int index;
		private Bounds bounds;
		private GameObject go;
		private GridGraph graph;
		
		public override void Reset() {
			useRealCollider = true;
			getWalkableOnly = false;
		}
   
		public override void OnEnter() {
			go = localGameObject.OwnerOption == OwnerDefaultOption.UseOwner ? Owner : localGameObject.GameObject.Value; // get the object that we want to use for the check
			bounds = go.renderer.bounds; // get object's bounds
			node = AstarPath.active.GetNearest(go.transform.position).node as Node; // get one node to start with Eg. for getting the graph type or for flooding it.
			
			if(node.GetType() == typeof(GridNode) )
			{
				Debug.Log("It's a grid node!");
				mohogonyGrid();
			}
			else 
			{
				mohogony();
				nodesOutput.Value = SetNodes(nodes);
			}
			
			//nodesOutput.Value = SetNodes(nodes) as FsmNodes;
			Debug.Log("Count in Area :" + (nodesOutput.Value as FsmNodes).Value.Count);
			Finish();
			return;
			
		} 
		
		public FsmNodes SetNodes(List<Node> go){ 
			FsmNodes doo = new FsmNodes();
			doo.Value = go;
			return doo;
		} 
		
		void mohogonyGrid() // this is the optimized version. Other graphs cannot be optimised this way because they have irregular structures unfortunately. Still, we shouldn't miss on the advantages of this specific graph type, so I included it specifically.
		{
			nodes = new List<Node>();
			nodes.Add(node as Node);
			graph = AstarPath.active.graphs[node.graphIndex] as GridGraph;
			

			GraphUpdateShape gus = new GraphUpdateShape(); 
			if (useRealCollider.Value)
			{

				if(go.collider.GetType() == typeof(BoxCollider)) // take render bounds, then turn them into world coordinates
				{
					Debug.Log("It's a box collider");
					bounds.center = (go.collider as BoxCollider).center;
					bounds.size = (go.collider as BoxCollider).size;
					
					calculateBox(gus);
				
				}
				else if(go.collider.GetType() == typeof(MeshCollider))
				{
					gus.points = (go.collider as MeshCollider).sharedMesh.vertices;
					for(var i=0; i < gus.points.Count(); i++ )
					{
						gus.points[i] = go.transform.TransformPoint((go.collider as MeshCollider).sharedMesh.vertices[i]);
					}
					Debug.Log("It's a mesh collider!");
				}
				else // any other collider
				{
					calculateBox(gus);
					Debug.Log("This type of collider is not specifically supported. Using bounds instead...");
				}
				
			}
			
			else // get the points of the render bounds
			{
				bounds = (go.renderer.GetComponent<MeshFilter>()).sharedMesh.bounds;
				calculateBox(gus);
			}
			
			//gus.convex = true;
			nodes = graph.GetNodesInArea(gus);
			
			if(getWalkableOnly.Value)
			{
				connected = new List<Node>();
				for(var i=0; i < nodes.Count(); i++ )
				{
					if(nodes[i].walkable)
					{
						connected.Add(nodes[i]); // connected is a substitute for a better named one :D There is no real sense behind the name here.
					}
				}
				nodesOutput.Value = SetNodes(connected);
			}
			
			else
			{
				nodesOutput.Value = SetNodes(nodes);
			}
			
			Debug.Log("i" + nodes.Count);


			return;
		}	
		
		void calculateBox(GraphUpdateShape gus) {
			gus.points = new Vector3[8];
			gus.points[0] = go.transform.TransformPoint(new Vector3(bounds.min.x, bounds.min.y, bounds.max.z));
			gus.points[1] = go.transform.TransformPoint(new Vector3(bounds.max.x, bounds.min.y, bounds.max.z));
			gus.points[2] = go.transform.TransformPoint(new Vector3(bounds.max.x, bounds.min.y, bounds.min.z));
			gus.points[3] = go.transform.TransformPoint(new Vector3(bounds.min.x, bounds.min.y, bounds.min.z));
			gus.points[7] = go.transform.TransformPoint(new Vector3(bounds.min.x, bounds.max.y, bounds.min.z));
			gus.points[4] = go.transform.TransformPoint(new Vector3(bounds.min.x, bounds.max.y, bounds.max.z));
			gus.points[5] = go.transform.TransformPoint(new Vector3(bounds.max.x, bounds.max.y, bounds.max.z));
			gus.points[6] = go.transform.TransformPoint(new Vector3(bounds.max.x, bounds.max.y, bounds.min.z));		
		}
		
		void mohogony() {
			
			//nodes = nodes.Union(FsmConverter.NodeListToArray(node.connections));
			nodes = new List<Node>();
			nodes.Add(node as Node);
			if(getWalkableOnly.Value) // actually, this does not get all walkables in the area, it just floods outwards along the walkables from the center of the gameObject. anyways, could be useful, so I left it in.
			{
				for(var i=0; i < nodes.Count; i++) 
				{
					//connected = nodes[i].connections.ToList(); does not work on gridgraphs, avoid using this :)
					nodes[i].GetConnections(CheckNode);
				}
			}
			
			else
			{
				alln = AstarPath.active.graphs[node.graphIndex].nodes; // get all nodes, no .ToList(); to save performance (save wherever you can :D )
				foreach (Node o in alln)
				{
					CheckNode(o);
				}
			}
			


			return;
		}
		
		
		public void CheckNode(Node o){

			if(useRealCollider.Value) 
			{
				if ((!nodes.Contains(o)) && IsInside(go.collider, (new Vector3(o.position.x,o.position.y,o.position.z) *  Int3.PrecisionFactor))) // check if the node in question is both in the collider and NOT in the list already.
				{
					nodes.Add(o);
				}			
			}
			else
			{
				if ((!nodes.Contains(o)) && bounds.Contains((new Vector3(o.position.x,o.position.y,o.position.z)) *  Int3.PrecisionFactor)) // check if the node in question is both in the bounds and NOT in the list already.
				{
					nodes.Add(o);
				}
			}
			

			return;
			
		}
		
		public bool IsInside ( Collider test, Vector3 point) // this is actually faster then the Bounnds.Contains. On any smaller scale this can speed it up from 3 to 60 frames :D
		{
			Vector3 center;
			Vector3 direction;
			Ray ray;
			RaycastHit hitInfo;
			bool hit;
			 
			// Use collider bounds to get the center of the collider. May be inaccurate
			// for some colliders (i.e. MeshCollider with a 'plane' mesh)
			center = test.bounds.center;
			 
			// Cast a ray from point to center
			direction = center - point;
			ray = new Ray(point, direction);
			hit = test.Raycast(ray, out hitInfo, direction.magnitude);
			 
			// If we hit the collider, point is outside. So we return !hit
			return !hit;
		}
		
	}
}