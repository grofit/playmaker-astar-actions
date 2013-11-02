using System;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Helpers;
using FsmPathfinding;
using Pathfinding;
using System.Linq;

namespace HutongGames.PlayMaker.Pathfinding
{
	[ActionCategory("A Star")]
	[Tooltip("set all kinds of variables in aany kind of graph, or choose a specific type of graph to set variables unique to that graphtype")]
	public class setGraphInfo : FsmStateAction
	{		
		[Tooltip("Choose the right type of graph. Choosing Any works on any graph, but choosing the wrong one will return nothing.")]	
		public GraphType graphType;
		
		[ActionSection("Input")]
		[RequiredField]
		[ObjectType(typeof(FsmNavGraph))]
		[Tooltip("The graph")]	
		public FsmObject graph;
		
        [UIHint(UIHint.Layer)]
        [Tooltip("Pick only from these layers in the raycast check")]
        public FsmInt mask;
		
		[Tooltip("	Max distance along the axis for a connection to be valid. ")]	
		public FsmVector3 limits;
			
		[Tooltip("	Size of the grid. ")]
		public FsmVector2 size;
		
		[Tooltip("draw general graph gizmos? Like nodes etc?")]	
		public FsmBool drawGizmos ;
		
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
				
		[Tooltip("Used in the editor to check if the info screen is open. ")]
		public FsmInt initialPenalty;
		
		[Tooltip("	In GetNearestForce, determines how far to search after a valid node has been found. ")]
		public FsmInt getNearestForceOverlap;

		public FsmInt scans;
			
		public FsmFloat thickRaycastRadius;
		
		[Tooltip("Max distance for a connection to be valid. ")]
		public FsmFloat maxDistance ;
		
		public FsmString name;
		
		[Tooltip("If no root is set, all nodes with the tag is used as nodes. ")]
		public FsmString searchTag;
		
		[Tooltip("All nodes this graph contains. They are not the same type as the nodes from the path, though they are extensions")]
		[ObjectType(typeof(FsmNodes))]
		public FsmObject nodes;
		
		public FsmBool everyFrame;
		
		private NavGraph g ;
     	  
		public override void Reset()
		{
			graph = null;
			mask = new FsmInt { UseVariable = true }; 
			limits = new FsmVector3 { UseVariable = true};
			size = new FsmVector2  {UseVariable = true};
			drawGizmos = new FsmBool {UseVariable = true};
			infoScreenOpen = new FsmBool {UseVariable = true};
			open = new FsmBool {UseVariable = true};
			autoLinkNodes = new FsmBool {UseVariable = true};
			raycast = new FsmBool {UseVariable = true};
			recursive = new FsmBool {UseVariable = true};
			thickRaycast = new FsmBool {UseVariable = true};
			initialPenalty = new FsmInt {UseVariable = true};
			getNearestForceOverlap = new FsmInt {UseVariable = true};
			scans = new FsmInt {UseVariable = true};
			thickRaycastRadius = new FsmFloat {UseVariable = true};
			maxDistance = new FsmFloat {UseVariable = true};
			name = new FsmString {UseVariable = true};
			searchTag = new FsmString {UseVariable = true};
			nodes = new FsmObject {UseVariable = true};
		}
		
		public override void OnEnter() 
	  	{			
			DoStuff();
			
			if (!everyFrame.Value)
			Finish();			
		}
	  
		public void DoStuff(){

			var go = graph.Value as FsmNavGraph;
			if(go.Value == null) 
			{
				Finish(); 
				return;
			}
					
			g = FsmConverter.GetNavGraph(graph);
			
			if (!drawGizmos.IsNone)
			{ g.drawGizmos = drawGizmos.Value; }
			
			if (!infoScreenOpen.IsNone)
			{ g.infoScreenOpen = infoScreenOpen.Value; }
			
			if (!initialPenalty.IsNone)
			{ g.initialPenalty = (uint)initialPenalty.Value; }
			
			if (!name.IsNone)
			{ g.name = name.Value; }
			
			if (!nodes.IsNone)
			{ g.nodes = FsmConverter.NodeArrayFromList((nodes.Value as FsmNodes).Value); }
			
			if (!open.IsNone)
			{ g.open = open.Value; }
			
			if(graphType == GraphType.pointGraph && g as PointGraph != null)
			{
				if (!autoLinkNodes.IsNone)
				{ (g as PointGraph).autoLinkNodes = autoLinkNodes.Value; }
				
				if (!limits.IsNone)
				{ (g as PointGraph).limits = limits.Value ; }
				
				if (!mask.IsNone)
				{ (g as PointGraph).mask = mask.Value ; }
				
				if (!maxDistance.IsNone)
				{ (g as PointGraph).maxDistance = maxDistance.Value; }
				
				if (!raycast.IsNone)
				{ (g as PointGraph).raycast = raycast.Value; }
				
				if (!recursive.IsNone)
				{ (g as PointGraph).recursive  = recursive.Value; }
				
				if (!searchTag.IsNone)
				{ (g as PointGraph).searchTag = searchTag.Value; }
				
				if (!thickRaycast.IsNone)
				{ (g as PointGraph).thickRaycast = thickRaycast.Value; }
				
				if (!thickRaycastRadius.IsNone)
				{ (g as PointGraph).thickRaycastRadius = thickRaycastRadius.Value ; }
			}
			
			if(graphType == GraphType.gridGraph && g as GridGraph != null)
			{
				if (!getNearestForceOverlap.IsNone)
				{ (g as GridGraph).getNearestForceOverlap = getNearestForceOverlap.Value ;}
				
				if (!scans.IsNone)
				{ (g as GridGraph).scans = scans.Value; }
				
				if (!size.IsNone)
				{ (g as GridGraph).size = size.Value; }
			}
		}
		
		public override void OnUpdate()
		{
			DoStuff();
		}	  
   	}
}