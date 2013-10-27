using System;
using HutongGames.PlayMaker;
using FsmPathfinding;
using Pathfinding;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding.Nodes;

namespace HutongGames.PlayMaker.Helpers
{
	public static class FsmConverter 
	{
		public static IList<Node> NodeListToArray(Node[] nodeArray)
		{ return new List<Node>(nodeArray); }
		
		public static Node[] NodeArrayFromList(IEnumerable<Node> nodeList)
		{ return nodeList.ToArray(); }
		
		public static Int3 v3i3(Vector3 position, Node node)
		{			
			var x = (int)(position.x/Int3.PrecisionFactor);
			var y = (int)(position.y/Int3.PrecisionFactor);
			var z = (int)(position.z/Int3.PrecisionFactor);
			return new Int3(x,y,z);
		}

		public static IList<GridNode> GridNodeToList(GridNode[] gridNodeArray)
		{ return gridNodeArray.ToList(); }
		
		public static Type FsmGetTypeOf(FsmPathfindingBase pathfinder) 
		{ return pathfinder.Value ? pathfinder.Value.GetType () : null; }
			
		public static bool FsmIsNull(FsmPathfindingBase pathfinder) 
		{
			try	
			{ return pathfinder.Value == null; }
			catch (InvalidCastException e) 
			{ return true; }
		}
		
		public static object GetAnythingShallow(FsmObject fsmObject) 
		{
			if(fsmObject == null || fsmObject.Value == null)
			{ return null; }
			
			if(fsmObject.Value is FsmNode)
			{ return GetNode(fsmObject); }
			else if(fsmObject.Value is FsmGridNode)
			{ return GetGridNode(fsmObject); }
			else if(fsmObject.Value is FsmGridNodes)
			{ return GetGridNodes(fsmObject); }
			else if(fsmObject.Value is FsmPath || fsmObject.Value is FsmABPath)
			{ return GetPath(fsmObject); }
			else if(fsmObject.Value is FsmPointGraph)
			{ return GetPointGraph(fsmObject); }
			else if(fsmObject.Value is FsmPointGraphs)
			{ return GetPointGraphs(fsmObject); }

			return null;
		}
		
		public static  AstarPath GetAstarPath(FsmObject gameObject)
		{
			var doo = gameObject.Value as FsmAstarPath;
			return doo.Value as AstarPath;
		}
		
		public static FsmAstarPath SetAstarPath(AstarPath gameObject)
		{ 
			return new FsmAstarPath
			{
				Value = gameObject 
			};
		}
		
		public static AstarData GetAstarData(FsmObject gameObject)
		{ 
			var doo = gameObject.Value as FsmAstarData;
			return doo.Value;
		}
		
		public static FsmAstarData SetAstarData(AstarData gameObject)
		{
			return new FsmAstarData()
			{
				Value = gameObject
			};
		}

		public static NavGraph[] GetNavGraphs(FsmObject gameObject)
		{ 
			var doo = gameObject.Value as FsmNavGraphs;
			return doo.Value;
		}
		
		public static FsmNavGraphs SetNavGraphs(NavGraph[] gameObject)
		{ 
			return new FsmNavGraphs()
			{
				Value = gameObject
			};
		}
		
		public static NavGraph GetNavGraph(FsmObject gameObject)
		{ 
			var doo = gameObject.Value as FsmNavGraph;
			return doo.Value;
		}
		
		public static FsmNavGraph SetNavGraph(NavGraph gameObject)
		{ 
			return new FsmNavGraph()
			{
				Value = gameObject
			};
		}

		public static PointGraph[] GetPointGraphs(FsmObject gameObject)
		{ 
			var doo = gameObject.Value as FsmPointGraphs;
			return doo.Value;
		}
		
		public static FsmPointGraphs SetPointGraphs(PointGraph[] gameObject)
		{ 
			return new FsmPointGraphs()
			{
				Value = gameObject
			};
		}
		
		public static PointGraph GetPointGraph(FsmObject gameObject)
		{ 
			var doo = gameObject.Value as FsmPointGraph;
			return doo.Value;
		}
		
		public static FsmPointGraph SetPointGraph(PointGraph gameObject)
		{ 
			return new FsmPointGraph()
			{
				Value = gameObject
			};
		}
		
		public static Node GetNode(FsmObject gameObject)
		{ 
			var doo = gameObject.Value as FsmNode;
			return doo.Value;
		}
		
		public static FsmNode SetNode(Node gameObject)
		{ 
			return new FsmNode()
			{
				Value = gameObject
			};
		}
		
		public static IList<Node> GetNodes(FsmObject gameObject)
		{ 
			var doo = gameObject.Value as FsmNodes;
			return doo.Value;
		}
		
		public static FsmNodes SetNodes(IList<Node> go)
		{ 
			return new FsmNodes()
			{
				Value = (List<Node>)go
			};
		}

		public static GridNode GetGridNode(FsmObject gameObject)
		{ 
			var doo = gameObject.Value as FsmGridNode;
			return doo.Value;
		}
		
		public static FsmGridNode SetGridNode(GridNode gameObject)
		{ 
			return  new FsmGridNode()
			{
				Value = gameObject
			};
		}
		
		public static GridNode[] GetGridNodes(FsmObject gameObject)
		{ 
			var doo = gameObject.Value as FsmGridNodes;
			return doo.Value;
		}
		
		public static FsmGridNodes SetGridNodes(GridNode[] gameObject) 
		{ 
			return new FsmGridNodes()
			{
				Value = gameObject
			};
		}

		public static Path GetPath(FsmObject gameObject)
		{ 
			var doo = gameObject.Value as FsmPath;
			var foo = doo.Value;
			return foo;
		}
		
		public static ABPath GetABPath(FsmObject gameObject) 
		{ 
			var doo = gameObject.Value as FsmPath;
			return doo.Value as ABPath;
		}
		
		public static FsmPath SetPath(Path gameObject)
		{ 
			return new FsmPath()
			{
				Value = gameObject
			};
		}
		
		public static Path[] GetPaths(FsmObject gameObject)
		{ 
			var doo = gameObject.Value as FsmPaths;
			return doo.Value;
		}
		
		public static FsmPaths SetPaths(Path[] gameObject) 
		{ 
			return new FsmPaths()
			{
				Value = gameObject
			};
		}
		
		public static NNConstraint GetNNConstraint(FsmObject gameObject)
		{ 
			var doo = gameObject.Value as FsmNNConstraint;
			return doo.Value;
		}
		
		public static FsmNNConstraint SetNNConstraint(NNConstraint gameObject)
		{ 
			return new FsmNNConstraint()
			{
				Value = gameObject
			};
		}
		
		public static NodeRunData GetNodeRunData(FsmObject gameObject)
		{ 
			var doo = gameObject.Value as FsmNodeRunData;
			return doo.Value;
		}
		
		public static FsmNodeRunData SetNodeRunData(NodeRunData gameObject)
		{ 
			return new FsmNodeRunData()
			{
				Value = gameObject
			};
		}
   	}
}