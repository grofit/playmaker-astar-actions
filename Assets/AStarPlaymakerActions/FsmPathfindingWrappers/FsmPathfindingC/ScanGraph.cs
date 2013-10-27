using UnityEngine;
using System.Collections;
using Pathfinding;
using FsmPathfinding;
using AstarPathExtension;

namespace HutongGames.PlayMaker.Actions
{
   [ActionCategory("A Star")]
   [Tooltip("Scans all graphs")]
   public class ScanGraph   : FsmStateAction 
   {
		[Tooltip("Scan this particular graph")]
		[ObjectType(typeof(FsmNavGraph))]
		public FsmObject graph;

		public override void OnEnter() {
			if(graph.Value != null && (graph.Value as FsmNavGraph).Value != null )
			{
				var g = (graph.Value as FsmNavGraph).Value as NavGraph;
				AstarPathExtensions.ScanGraph(g);
			}
			Finish();
				
		} 
	}

}