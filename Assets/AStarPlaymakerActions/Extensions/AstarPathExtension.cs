using Pathfinding;

namespace HutongGames.PlayMaker.Behaviours
{
    public static class AstarPathExtensions 
    {
	    public static void ScanGraph (this NavGraph graph) 
        {
		    if (AstarPath.OnGraphPreScan != null) 
            { AstarPath.OnGraphPreScan (graph); }
		
		    graph.Scan ();
		
		    var index = AstarPath.active.astarData.GetGraphIndex (graph);
		    if (index < 0)
            { throw new System.ArgumentException ("Graph is not added to AstarData"); }
		
		    if (graph.nodes != null) 
            {
			    for (int j=0;j<graph.nodes.Length;j++) 
                {
				    if (graph.nodes[j] != null) 
					{ graph.nodes[j].graphIndex = index; }
			    }
		    }
		
		    if (AstarPath.OnGraphPostScan != null) 
            { AstarPath.OnGraphPostScan (graph); }
		
		    AstarPath.active.FloodFill ();
		    AstarPath.active.DataUpdate ();
	    }
    }
}