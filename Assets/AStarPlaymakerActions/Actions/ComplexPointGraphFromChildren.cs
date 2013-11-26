using System.Linq;
using HutongGames.PlayMaker.Behaviours;
using FsmPathfinding;
using Pathfinding;

namespace HutongGames.PlayMaker.Pathfinding
{
	[ActionCategory("A Star")]
	[Tooltip("Recursively checks all children of a gameObject and adds their positions to a PointGraph. It then connects every node to surrounding nodes based on the max distance value. It's called complex because it creates a complex graph without any real order, and nodes can have all number of connections.")]
	public class ComplexPointGraphFromChildren : FsmStateAction
	{
		[ActionSection("Input")]
		[RequiredField]
		[ObjectType(typeof(FsmNavGraph))]
		[Tooltip("The point graph. Both input and output. But the variable does not actually need to contain a root yet, if there is none it will always create a new one. It just needs somewhere to save it then, which is why this is required.")]
		public FsmObject graph;
		
		[Tooltip("You can give this graph whatever name you want. If you want to get it by its' name lateron thouh, make sure that the name is unique.")]	
      	public FsmString name; 		
			
		[RequiredField]
		[Tooltip("The root of the gameObjects you want to use as a graph")]
      	public FsmOwnerDefault gameObject;  
		
		[Tooltip("How easy are the nodes to walk over? If it's a path through the swamp, then you may want to turn this up. Movement actions also have the option to move more slowly on nodes with high cost, and astar generally avoids them unless the cost is less than the cost of the additional nodes needed to go around them (in this case going around the swamp) would take.")	]
		public FsmInt cost;
		
		[Tooltip("If the distance between 2 nodes is less than this, they will be connected")]
		public FsmFloat maxDistance;
		
		[Tooltip("Check this to connect the graph nodes to any node from a different graph, if those nodes are closer than the Max Distance") ]
		public FsmBool connect;
		
		[ActionSection("Output : Nodes...s")]
		[Tooltip("If you're using a grid graph, this will be what you need.")]
		[ObjectType(typeof(FsmNodes))]
		public FsmObject Nodes; 
		
		[Tooltip("If true, this always creates a new PointGraph. If False, this adds to the current PointGraph in the graph variable.") ]
		public FsmBool alwaysNew;
		
		public override void Reset()
		{
			graph = null;
			maxDistance = 1.5f;	
		}
		
		public override void OnEnter()
	  	{
			if(graph.Value == null)
            { graph.Value = new FsmNavGraph(); }
			
			var fsmNavGraph = graph.Value as FsmNavGraph;

            PointGraph pointGraph;
			if (fsmNavGraph.Value == null || alwaysNew.Value) 
			{
				pointGraph = AstarPath.active.astarData.AddGraph( typeof( PointGraph )) as PointGraph;	
				graph.Value = new FsmNavGraph { Value = pointGraph };
			}
			else 
            { 
				pointGraph = graph.GetNavGraph() as PointGraph;
				if(pointGraph==null)
                { throw new System.ArgumentException("The input graph variable does not contain a Pointgraph, but some other type of graph."); }
			}
			
			ScanPointGraph(pointGraph);
			Finish();			
		}
		
		public void ScanPointGraph(PointGraph pointGraph)
		{
		 	var go = gameObject.OwnerOption == OwnerDefaultOption.UseOwner ? Owner : gameObject.GameObject.Value;
		 	
		 	pointGraph.root = go.transform;
		 	pointGraph.maxDistance = maxDistance.Value;
			pointGraph.initialPenalty = (uint)cost.Value;
			pointGraph.name = name.Value;
            pointGraph.ScanGraph();

            Nodes.Value = new FsmNodes { Value = pointGraph.nodes.ToList() };
			AstarPath.active.FloodFill ();
		}		  
   	}	
}