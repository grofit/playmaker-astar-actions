using System;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Helpers;
using FsmPathfinding;
using Pathfinding;
using System.Linq;
using UnityEngine;

namespace HutongGames.PlayMaker.Pathfinding
{
	[ActionCategory("A Star")]
	[Tooltip("Using the Seeker component on a GameObject a path is calculated and then followed.")]
	public class getNodeInfo : HutongGames.PlayMaker.FsmStateAction{

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
      
		public void Reset()
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
	  
		public void OnEnter() 
	  	{			
			var mo = node.Value as FsmNode;
			Debug.Log(mo);
			
			if(mo == null || mo.Value == null) 
			{
				Debug.Log("input invalid. Make sure the node is properly assigned.");
				Finish();
				return;
			}
			DoStuff();
			
			if (!everyFrame.Value)
			{ Finish(); }			
		}
	  
		public void DoStuff()
		{		
			var doo = ((node as FsmObject).Value as FsmNode).Value as Node;
			
			nodeIndex.Value = doo.GetNodeIndex();			
			penalty.Value = (int)doo.penalty;			
			area.Value = doo.area;			
			tags.Value = doo.tags;			
			walkable.Value = doo.walkable;			
			graphIndex.Value = doo.graphIndex;			
			position.Value = new Vector3(doo.position.x,doo.position.y,doo.position.z);
			position.Value *= Int3.PrecisionFactor;
			
			if (!connectedNodes.IsNone)
			(connectedNodes.Value as FsmNodes).Value = (List<Node>)FsmConverter.NodeListToArray(doo.connections);
			
			var loo = new FsmNavGraph();
			loo.Value = AstarPath.active.graphs[doo.graphIndex];
			graph.Value = loo;
		}
		
		public void OnUpdate()
		{
			DoStuff();
		}
   	}
}