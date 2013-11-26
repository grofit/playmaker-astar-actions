using System;
using System.Linq;
using FsmPathfinding;
using HutongGames.PlayMaker.Extensions;
using HutongGames.PlayMaker.Pathfinding.Enums;
using Pathfinding;

namespace HutongGames.PlayMaker.Pathfinding
{
    [ActionCategory("A Star")]
	[Tooltip("Using the Seeker component on a GameObject a path is calculated and then followed.")]
	public class GetGraphInfo : FsmStateAction
	{		
		[Tooltip("Choose the right type of graph. Choosing Any works on any graph, but choosing the wrong one will return nothing.")]	
		public GraphType graphType;
		
		[ActionSection("Input")]
		[RequiredField]
		[ObjectType(typeof(FsmNavGraph))]
		[Tooltip("The graph")]
		public FsmObject graph;
		
		[ActionSection ("Output - GameObjects")]
		[Tooltip("Childs of this transform are treated as nodes. ")	]
		public FsmGameObject root;
		
		[ActionSection ("Output - Layermasks")]
        [UIHint(UIHint.Layer)]
        [Tooltip("Pick only from these layers in the raycast check")]
        public FsmInt mask;
		
		[ActionSection("Vector3")]
		[Tooltip("	Max distance along the axis for a connection to be valid. ")]	
		public FsmVector3 limits;
		
		[ActionSection ("Vector2")	]
		[Tooltip("	Size of the grid. ")	]	
		public FsmVector2 size;
		
		[ActionSection("Output - Strings")]
		[Tooltip("Returns unique ID of this graph")	]
		public FsmString guid ;
		
		[ActionSection ("Bools")]
		[Tooltip("draw general graph gizmos? Like nodes etc?")	]
		public FsmBool drawGizmos;
		
		[Tooltip("Used in the editor to check if the info screen is open. ")]	
		public FsmBool infoScreenOpen;
		
		[Tooltip("	Is the graph open in the editor ")]
		public FsmBool open;

		public FsmBool autoLinkNodes;
		
		[Tooltip("Use raycasts to check connections. ")]
		public FsmBool raycast;

		[Tooltip("Recursively search for childnodes to the root. ")]
		public FsmBool recursive;
		
		[Tooltip("Use thick raycast. ")]
		public FsmBool thickRaycast;
		
		[ActionSection ("Ints")		]
		[Tooltip("Used in the editor to check if the info screen is open. ")	]
		public FsmInt initialPenalty;
		
		[Tooltip("	In GetNearestForce, determines how far to search after a valid node has been found. ")	]
		public FsmInt getNearestForceOverlap;

		public FsmInt scans;
		
		[ActionSection ("Floats")]		
		public FsmFloat thickRaycastRadius;
		
		[Tooltip("Max distance for a connection to be valid. ")]
		public FsmFloat maxDistance;
		
		[ActionSection ("Strings")]
		public FsmString name;
		
		[Tooltip("If no root is set, all nodes with the tag is used as nodes. ")]
		public FsmString searchTag;
		
		[ActionSection ("Nodes...s")]
		[Tooltip("All nodes this graph contains. They are not the same type as the nodes from the path, though they are extensions")]
		[ObjectType(typeof(FsmNodes))]
		public FsmObject nodes;
		
		public FsmBool everyFrame;
		
		public override void Reset()
		{
			graph = null;
			guid = null;
		}
		
		public override void OnEnter() 
	  	{
			GetInfoFromGraph();
			
			if (!everyFrame.Value)
			Finish();			
		}
	  
		public void GetInfoFromGraph()
		{
			var underlyingFsmNavGraph = graph.Value as FsmNavGraph;
            if(underlyingFsmNavGraph == null)
            { throw new NullReferenceException("Graph does not contain any underlying graph data"); }

			if(underlyingFsmNavGraph.Value == null) 
			{
				Finish(); 
				return;
			}			
			
			var currentNavGraph = graph.GetNavGraph();
			guid.Value = currentNavGraph.guid.ToString();
			drawGizmos.Value = currentNavGraph.drawGizmos;
			infoScreenOpen.Value = currentNavGraph.infoScreenOpen;
			initialPenalty.Value = (int)currentNavGraph.initialPenalty;
			name.Value = currentNavGraph.name;
            nodes.Value = new FsmNodes { Value = currentNavGraph.nodes.ToList() };
			open.Value = currentNavGraph.open;
			
			if(graphType == GraphType.PointGraph && currentNavGraph as PointGraph != null)
			{
				autoLinkNodes.Value = (currentNavGraph as PointGraph).autoLinkNodes ;
				limits.Value = (currentNavGraph as PointGraph).limits ;
				mask.Value =  (currentNavGraph as PointGraph).mask ;
				maxDistance.Value = (currentNavGraph as PointGraph).maxDistance ;
				raycast.Value = (currentNavGraph as PointGraph).raycast ;
				recursive.Value = (currentNavGraph as PointGraph).recursive ;
				root.Value = (currentNavGraph as PointGraph).root.gameObject ;
				searchTag.Value = (currentNavGraph as PointGraph).searchTag ;
				thickRaycast.Value = (currentNavGraph as PointGraph).thickRaycast ;
				thickRaycastRadius.Value = (currentNavGraph as PointGraph).thickRaycastRadius ;
			}
			
			if(graphType == GraphType.GridGraph && currentNavGraph as GridGraph != null)
			{
				getNearestForceOverlap.Value = (currentNavGraph as GridGraph).getNearestForceOverlap ;
				scans.Value = (currentNavGraph as GridGraph).scans ;
				size.Value = (currentNavGraph as GridGraph).size;
			}
		}
		
		public override void OnUpdate()
		{ GetInfoFromGraph(); }	  
   	}
}