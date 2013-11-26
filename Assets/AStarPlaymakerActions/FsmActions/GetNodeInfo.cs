using System.Linq;
using HutongGames.PlayMaker.Pathfinding;
using Pathfinding;
using UnityEngine;

namespace HutongGames.PlayMaker.Pathfinding
{
	[ActionCategory("A Star")]
	[Tooltip("Using the Seeker component on a GameObject a path is calculated and then followed.")]
	public class GetNodeInfo : FsmStateAction{

		[ActionSection("Input")]
		[RequiredField]
		[ObjectType(typeof(FsmNode))]
		[Tooltip("The node")]	
		public FsmObject node;
		
		[ActionSection("Output")]
		[Tooltip("Returns the global node index")	]
		public FsmInt nodeIndex;
		
		[Tooltip("Penlty cost for walking on this node. ")	]
		public FsmInt penalty;
		
		[Tooltip("Area ID of the node. ")]	
		public FsmInt area;
		
		[Tooltip("The index of the graph this node is in. ")]	
		public FsmInt graphIndex;
		
		[Tooltip("Tags for walkability.")	]
		public FsmInt tags;
		
		[Tooltip("Tags for walkability.")	]
		public FsmBool walkable;
		
		[Tooltip("Is the node walkable. ")]		
		public FsmVector3 position;
		
		[Tooltip("Nodes : every node that is connected to this one.")]	
		public FsmObject connectedNodes;
		
		[Tooltip ("The graph the node is contained in")]
		[ObjectType(typeof(FsmNavGraph))]
		public FsmObject graph;

		public FsmBool everyFrame;		
      
		public override void Reset()
		{
			node = new FsmObject();
			nodeIndex = null;
			penalty = null;
			area = null;
			graphIndex = null;
			tags = null;
			walkable = null;
			position = null;
			connectedNodes = new FsmObject(); 
			connectedNodes.UseVariable = true;
		}
	  
		public override void OnEnter() 
	  	{			
			var underlyingFsmNode = node.Value as FsmNode;
			if(underlyingFsmNode == null || underlyingFsmNode.Value == null) 
			{
				Debug.Log("input invalid. Make sure the node is properly assigned.");
				Finish();
				return;
			}

			GetInfoFromNode();
			
			if (!everyFrame.Value)
			{ Finish(); }			
		}
	  
		public void GetInfoFromNode()
		{		
			var underlingNode = (node.Value as FsmNode).Value;
			
			nodeIndex.Value = underlingNode.GetNodeIndex();			
			penalty.Value = (int)underlingNode.penalty;			
			area.Value = underlingNode.area;			
			tags.Value = underlingNode.tags;			
			walkable.Value = underlingNode.walkable;			
			graphIndex.Value = underlingNode.graphIndex;			
			position.Value = new Vector3(underlingNode.position.x,underlingNode.position.y,underlingNode.position.z);
			position.Value *= Int3.PrecisionFactor;
			
			if (!connectedNodes.IsNone)
			{ (connectedNodes.Value as FsmNodes).Value = underlingNode.connections.ToList(); }

		    var currentNavGraph = AstarPath.active.graphs[underlingNode.graphIndex];
            var newNavGraph = new FsmNavGraph { Value = currentNavGraph };
		    graph.Value = newNavGraph;
		}
		
		public override void OnUpdate()
		{
			GetInfoFromNode();
		}
   	}
}