using System;
using HutongGames.PlayMaker.Pathfinding;
using HutongGames.PlayMaker.Extensions;

namespace HutongGames.PlayMaker.Pathfinding
{
	[ActionCategory("A Star")]
	[Tooltip("Using the Seeker component on a GameObject a path is calculated and then followed.")]
	public class GetGraphInGraphs : FsmStateAction
	{
		[ActionSection("Input")]
		[RequiredField]
		[ObjectType(typeof(FsmNavGraphs))]
		[Tooltip("Constraint for how to search for graphs. ")	]
		public FsmObject graphs;
		
		[RequiredField]
		[Tooltip("Index of the graph")	]
		public FsmInt index;
		
		[ActionSection("Output")]
		[ObjectType(typeof(FsmNavGraph))]
		[Tooltip("The graph ")	]
		public FsmObject graph;

		public override void Reset()
		{
			graphs = new FsmObject();
			index = 0;
			graph = new FsmObject();
		}
	  
		public override void OnEnter() 
	  	{
			var underlyingFsmNavGraphs = graphs.Value as FsmNavGraphs;
            if(underlyingFsmNavGraphs == null)
            { throw new NullReferenceException("The graphs contains no underlying data"); }

		    if ((underlyingFsmNavGraphs.Value == null) || !graph.UseVariable)
		    {
		        Finish(); 
                return;
		    }
						
			var navGraphArray = graphs.GetNavGraphs();
            if (index.Value >= navGraphArray.Length)
            { Finish(); }

			var navGraphContainer = new FsmNavGraph { Value = navGraphArray[index.Value] };
            graph.Value = navGraphContainer;

			Finish();
		}
   	}
}