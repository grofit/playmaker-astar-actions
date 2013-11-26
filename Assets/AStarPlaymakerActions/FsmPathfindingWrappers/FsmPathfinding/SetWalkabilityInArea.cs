using UnityEngine;
using System.Collections;
using Pathfinding;
using Pathfinding.Nodes;
using System.Collections.Generic;
using System.Linq;
using FsmPathfinding;

namespace HutongGames.PlayMaker.Actions
{
   [ActionCategory("A Star")]
   [Tooltip("This action sets all nodes within a certain area that is defined by either a gameObject's bounds(outer most points forming a cube) or by it's collider. Use Update graph as often as possible, as that one is cheaper! ")]
   public class SetWalkabilityInArea   : FsmStateAction 
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
		private List<Node> connected;
		private Node[] alln;
		private int index;
		private Bounds bounds;
		private GameObject go;
		private GridGraph graph;
   
		public override void Reset() {
			useRealCollider = true;
			walkability = true;
		}
   
		public override void OnEnter() {
			go = localGameObject.OwnerOption == OwnerDefaultOption.UseOwner ? Owner : localGameObject.GameObject.Value;
			bounds = go.renderer.bounds; // get object's bounds
			node = AstarPath.active.GetNearest(go.transform.position).node as Node; // get one node to start with
			Debug.Log(node as Node);
			
			if(node.GetType() == typeof(GridNode) )
			{
				Debug.Log("It's a grid node!");
				mohogonyGrid();
			}
			
			else
			{
				mohogony();
			}
			

			Debug.Log("Count in Area :" + nodes.Count);
				
			Finish();
			return;
			
		} 
		
		void mohogonyGrid() // this is the optimized version. Other graphs cannot be optimised this way because they have irregular structures unfortunately. Still, we shouldn't miss on the advantages of this specific graph type, so I included it.
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
			
			gus.convex = true;
			nodes = graph.GetNodesInArea(gus);
			

			foreach(Node o in nodes )
			{
				o.walkable = walkability.Value;
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
			

			alln = AstarPath.active.graphs[node.graphIndex].nodes; // get all nodes, no .ToList(); to save performance (save wherever you can :D )
			foreach (Node o in alln)
			{
				CheckNode(o);
			}

			
			Debug.Log("i" + nodes.Count);


			return;
		}
		
		
		public void CheckNode(Node o){

			if(useRealCollider.Value) 
			{
				if ((!nodes.Contains(o)) && IsInside(go.collider, (new Vector3(o.position.x,o.position.y,o.position.z) *  Int3.PrecisionFactor))) // check if the node in question is both in the bounds and NOT in the list already.
				{
					o.walkable = walkability.Value;
				}			
			}
			else
			{
				if ((!nodes.Contains(o)) && bounds.Contains((new Vector3(o.position.x,o.position.y,o.position.z)) *  Int3.PrecisionFactor)) // check if the node in question is both in the bounds and NOT in the list already.
				{
					o.walkable = walkability.Value;
				}
			}
			

			return;
			
		}
		
		public bool IsInside ( Collider test, Vector3 point)
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