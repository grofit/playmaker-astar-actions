using System;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Helpers;
using FsmPathfinding;
using Pathfinding;
using System.Linq;

namespace HutongGames.PlayMaker.Pathfinding
{
	public enum GraphType
	{
		any,
		pointGraph,
		gridGraph,
		all,
	}
	
	[ActionCategory("A Star")]
	[Tooltip("Using the Seeker component on a GameObject a path is calculated and then followed.")]
	public class getGraphInfo : FsmStateAction
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
		
		private NavGraph g;
		
		public void Reset()
		{
			graph = null;
			guid = null;
		}
		
		public void OnEnter() 
	  	{
			DoStuff();
			
			if (!everyFrame.Value)
			Finish();			
		}
	  
		public void DoStuff()
		{
			var go = graph.Value as FsmNavGraph;
			if(go.Value == null) 
			{
				Finish(); 
				return;
			}			
			
			g = FsmConverter.GetNavGraph(graph);
			
			guid.Value = g.guid.ToString();
			drawGizmos.Value = g.drawGizmos;
			infoScreenOpen.Value = g.infoScreenOpen;
			initialPenalty.Value = (int)g.initialPenalty;
			name.Value = g.name;

			nodes.Value = FsmConverter.SetNodes(FsmConverter.NodeListToArray(g.nodes)) as FsmNodes;  // everywhere else it's saved as a generic list, only here it is an array, so it needs extra conversion
			open.Value = g.open;
			
			if(graphType == GraphType.pointGraph && g as PointGraph != null)
			{
				autoLinkNodes.Value = (g as PointGraph).autoLinkNodes ;
				limits.Value = (g as PointGraph).limits ;
				mask.Value =  (g as PointGraph).mask ;
				maxDistance.Value = (g as PointGraph).maxDistance ;
				raycast.Value = (g as PointGraph).raycast ;
				recursive.Value = (g as PointGraph).recursive ;
				root.Value = (g as PointGraph).root.gameObject ;
				searchTag.Value = (g as PointGraph).searchTag ;
				thickRaycast.Value = (g as PointGraph).thickRaycast ;
				thickRaycastRadius.Value = (g as PointGraph).thickRaycastRadius ;
			}
			
			if(graphType == GraphType.gridGraph && g as GridGraph != null)
			{
				getNearestForceOverlap.Value = (g as GridGraph).getNearestForceOverlap ;
				scans.Value = (g as GridGraph).scans ;
				size.Value = (g as GridGraph).size;
			}
		}
		
		public void OnUpdate()
		{ DoStuff(); }	  
   	}
}