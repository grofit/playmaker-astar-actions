using HutongGames.PlayMaker.Helpers;
using FsmPathfinding;

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
			var go = graphs.Value as FsmNavGraphs;
			if((go.Value == null) || !graph.UseVariable) {Finish(); return;} // it would continue for a frame without return
						
			var goo = FsmConverter.GetNavGraphs(graphs);
			var coo = new FsmNavGraph();
			
			if(index.Value>=goo.Length) //check if the index exists
			{ Finish(); }
			
			coo.Value = goo[index.Value];
			graph.Value = coo;
			Finish();
		}
   	}
}