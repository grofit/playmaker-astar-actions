using System.Collections.Generic;
using HutongGames.PlayMaker.Helpers;
using FsmPathfinding;
using Pathfinding;
using AstarPathExtension;

namespace HutongGames.PlayMaker.Pathfinding
{
	[ActionCategory("A Star")]
	[Tooltip("Creates a number of nodes with the correct type for the graph. ")]
	public class createNodes : FsmStateAction
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
		
		private NavGraph g;
      
		public override void Reset()
		{
			graph = new FsmObject();
			nodes = new FsmObject();
			number = 0;
			everyFrame = false;	
		}
		
		public override void OnEnter() 
	  	{
			var mo = graph.Value as FsmNavGraph;
			if(mo.Value == null) 
			{
				Finish(); 
				return;
			} // it would continue for a frame without return
			
			g = FsmConverter.GetNavGraph(graph);
			
			DoStuff();
			
			if (!everyFrame.Value)
			{ Finish(); }

			
		}
		
		public void DoStuff()
		{
			var a = g.CreateNodes(number.Value);
			
			nodes.Value = new FsmNodes();
			
			(nodes.Value as FsmNodes).Value = (List<Node>)FsmConverter.NodeListToArray(a);
			
		//	g.nodes += a;
			
		//	AstarPath.active.NodeCountChanged() ;

			AstarPathExtensions.ScanGraph(g);

		}

		
		public override void OnUpdate()
		{
			DoStuff();
		} 
   	}
}
