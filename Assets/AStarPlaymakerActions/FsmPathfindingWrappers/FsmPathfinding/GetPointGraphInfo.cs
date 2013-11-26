using UnityEngine;
using System.Collections;
using System;
using HutongGames.PlayMaker;
using FsmPathfinding;
using Pathfinding;
using System.Linq;

namespace HutongGames.PlayMaker.Actions
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
		
		private PointGraph g;
	// Use this for initialization
	public override void OnEnter () {
			FsmNavGraph go = graph.Value as FsmNavGraph;
			if(go.Value as PointGraph == null) {Finish(); return;}
			g = go.Value as PointGraph;
			
			DoStuff();
			
			if (!everyFrame.Value)
			Finish();	
	}
	
		public void DoStuff(){
			guid.Value = g.guid.ToString();
			drawGizmos.Value = g.drawGizmos;
			infoScreenOpen.Value = g.infoScreenOpen;
			initialPenalty.Value = (int)g.initialPenalty;
			name.Value = g.name;
			var tempNodes = new FsmNodes {Value = g.nodes.ToList()};
		    nodes.Value = tempNodes;  // everywhere else it's saved as a generic list, only here it is an array, so it needs extra conversion
			open.Value = g.open;


		}
		
	// Update is called once per frame
		public override void OnUpdate()
		{
			DoStuff();
		}
}
}