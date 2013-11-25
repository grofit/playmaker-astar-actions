using System;
using System.Collections.Generic;
using System.Linq;
using HutongGames.PlayMaker.Helpers;
using FsmPathfinding;
using Pathfinding;
using AstarPathExtension;

namespace HutongGames.PlayMaker.Pathfinding
{
	[ActionCategory("A Star")]
	[Tooltip("Creates a number of nodes with the correct type for the graph. ")]
	public class CreateNodes : FsmStateAction
	{
		[ActionSection("Input")]
		[RequiredField]
		[ObjectType(typeof(FsmNavGraph))]
		[Tooltip("The graph")]
		public FsmObject graph;
			
		[Tooltip("Used in the editor to check if the info screen is open. ")]	
		public FsmInt number;
		
		[ActionSection ("Output : Nodes...s")]
		[Tooltip("A list of the newly created nodes")]
		[ObjectType(typeof(FsmNodes))]
		public FsmObject nodes;
		
		public FsmBool everyFrame;
		
		private NavGraph cachedNavGraph;
      
		public override void Reset()
		{
			graph = new FsmObject();
			nodes = new FsmObject();
			number = 0;
			everyFrame = false;	
		}
		
		public override void OnEnter() 
	  	{
			var fsmNavGraph = graph.Value as FsmNavGraph;
            if (fsmNavGraph == null || fsmNavGraph.Value == null) 
			{
				Finish(); 
				return;
			}
			
			cachedNavGraph = graph.GetNavGraph();
			CreateNodesForGraph();
			
			if (!everyFrame.Value)
			{ Finish(); }
		}
		
		public void CreateNodesForGraph()
		{
			var createdNodes = cachedNavGraph.CreateNodes(number.Value);
			nodes.Value = new FsmNodes();
			(nodes.Value as FsmNodes).Value = createdNodes.ToList();
			AstarPathExtensions.ScanGraph(cachedNavGraph);
		}
		
		public override void OnUpdate()
		{
			CreateNodesForGraph();
		} 
   	}
}
