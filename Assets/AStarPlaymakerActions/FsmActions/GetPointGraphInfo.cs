using FsmPathfinding;
using Pathfinding;
using System.Linq;

namespace HutongGames.PlayMaker.Pathfinding
{
    public class GetPointGraphInfo : FsmStateAction
    {
		[ActionSection("Input")]
		[RequiredField]
		[ObjectType(typeof(FsmNavGraph))]
		[Tooltip("The graph")]
		public FsmObject graph;
		
		[ActionSection ("Transforms")]
		public FsmObject root;
		
		[ActionSection ("Vector3")]
		[Tooltip("Max distance along the axis for a connection to be valid. ")]	
		public FsmVector3 limits;
		
		[ActionSection("Output - Strings")]
		[Tooltip("Returns unique ID of this graph")]	
		public FsmString guid;
		
		[ActionSection ("Bools")]
		[Tooltip("draw general graph gizmos? Like nodes etc?")]	
		public FsmBool drawGizmos;

		[Tooltip("Use raycasts to check connections.")]	
		public FsmBool raycast;

		[Tooltip("	Recursively search for childnodes to the root. ")]	
		public FsmBool recursive;

		[Tooltip("Use thick raycast. ")]	
		public FsmBool thickRaycast;		 
		
		public FsmBool autoLinkNodes;
		
		[Tooltip("Used in the editor to check if the info screen is open. ")]	
		public FsmBool infoScreenOpen;
		
		[Tooltip("	Is the graph open in the editor ")]
		public FsmBool open;
		
		[ActionSection ("Ints")]		
		[Tooltip("Used in the editor to check if the info screen is open. ")]	
		public FsmInt initialPenalty;
		
		[ActionSection ("Floats")]		
		[Tooltip("	Max distance for a connection to be valid. ")]	
		public FsmFloat maxDistance;		

		public FsmFloat thickRaycastRadius;		
		
		[ActionSection ("Strings")]
		public FsmString name;
		
		[Tooltip("If no root is set, all nodes with the tag is used as nodes. ")]
		public FsmString searchTag;
		
		[ActionSection ("Nodes...s")]
		[Tooltip("All nodes this graph contains. They are not the same type as the nodes from the path, though they are extensions")]
		[ObjectType(typeof(FsmNodes))]
		public FsmObject nodes;
		
		public FsmBool everyFrame;
		
		private PointGraph pointGraph;

	    public override void OnEnter () 
        {
            var navGraph = graph.Value as FsmNavGraph;
	        pointGraph = navGraph.Value as PointGraph;
	        if (pointGraph == null)
	        {
	            Finish(); 
                return;
	        }
			
            GetGraphInfo();
			
            if (!everyFrame.Value)
            { Finish(); }	
	    }
	
		public void GetGraphInfo()
        {
			guid.Value = pointGraph.guid.ToString();
			drawGizmos.Value = pointGraph.drawGizmos;
			infoScreenOpen.Value = pointGraph.infoScreenOpen;
			initialPenalty.Value = (int)pointGraph.initialPenalty;
			name.Value = pointGraph.name;
            nodes.Value = new FsmNodes { Value = pointGraph.nodes.ToList() }; ;
			open.Value = pointGraph.open;
		}
		
		public override void OnUpdate()
		{ GetGraphInfo(); }
    }
}