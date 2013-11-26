using FsmPathfinding;

namespace HutongGames.PlayMaker.Pathfinding
{
   [ActionCategory("A Star")]
   [Tooltip("Scans all graphs")]
   public class ScanGraph   : FsmStateAction 
   {
		[Tooltip("Scan this particular graph")]
		[ObjectType(typeof(FsmNavGraph))]
		public FsmObject graph;

		public override void OnEnter()
		{
		    var castGraph = graph.Value as FsmNavGraph;
            if (castGraph != null)
			{ castGraph.Value.ScanGraph(); }
			Finish();
		} 
	}
}