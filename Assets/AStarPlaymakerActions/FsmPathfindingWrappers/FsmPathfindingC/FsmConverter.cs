using System;
using UnityEngine;
using HutongGames.PlayMaker;
using FsmPathfinding;
using Pathfinding;
using System.Linq;
using System.Collections.Generic;


//This script saves all kinds of conversions. Mostly from FsmObject to a specific type, but it also has voids to check for a variable being null and to convert certain arrays to certain generic lists and vice versa

namespace HutongGames.PlayMaker.Actions
{
public abstract class FsmConverterC : FsmStateAction { // leave this as separate voids unless you want it to become terribly messy. Been there done that ;D

// real convertors first:
// Single array to generic list :
		public Node[] NodeListToArray(List<Node> go){
			Node[] outo = new Node[go.Count-1];
			
			
			int x = 0;
			while (x<go.Count)
			{
				outo[x] = go[x];
				x++;

			}
			
			return outo;
		
		}
		
		
		public List<Node> NodeArrayToList(Node[] go){
			
			List<Node> outo = new List<Node>();
			int x = 0;
			while (x<go.Length)
			{
				outo.Add(go[x]);
				x++;

			}
			
			return(outo);
		
		}

	/*
		public Int3 v3i3(UnityEngine.Vector3 go,Node doo ){
			float prf = Int3.PrecisionFactor;
			return (new Int3((go.x/prf) as int,(go.y/prf) as int,(go.z/prf)));
		}

		public List<Pathfinding.Nodes.GridNode> GridNodeToList(Pathfinding.Nodes.GridNode[] go){
			List<Pathfinding.Nodes.GridNode> outo;
			
			var x = 0;
			while (x<go.Length)
			{
				outo[x] = go[x];
				x++;
			}
			
			return outo;
		
		}
		*/
/*		
		public void FsmGetTypeOf(go) {
			if(go){
				var doo = (go as FsmPathfindingBase).Value;
				return typeof(doo.Value);
			}
		}
		
		public void GetAnything(go) {
			var typo = FsmGetTypeOf(go);	
			
			if (typo == Node) { //Node
				return GetNode(go);
			}
			
			if (typo == Nodes.GridNode) { //GridNode
				return GetGridNode(go);
			}
			return null;
		}
		
		public void GetAnythingShallow(mo) {
			var go = mo as FsmObject;
			var typo = typeof(go.Value);	
			
			if (typo == FsmNode) { //Node
				//Debug.Log(typo);
				return GetNode(go) as Node;
			}
			
			else if (typo == FsmGridNode) { //GridNode
				//Debug.Log("GridNode");
				return GetGridNode(go) as Nodes.GridNode;
			}
			
			else if (typo == FsmNodes) { //Node[]
				//Debug.Log("Nodes");
				return GetNodes(go) as Node[];
			}
			
			else if (typo == FsmGridNodes) { //GridNode[]
				//Debug.Log("GridNodes");
				return GetGridNodes(go) as Nodes.GridNode[];
			}
			
			else if (typo == FsmPath) { //Path
				//Debug.Log("Path");
				return GetPath(go) as Path;
			}
		
			else if (typo == FsmABPath) { //ABPath
				//Debug.Log("ABPath");
				return GetPath(go) as ABPath;
			}
			
			else if (typo == FsmPointGraph) { //PointGraph
				//Debug.Log("PointGraph");
				return GetPointGraph(go) as PointGraph;
			}
		
			else if (typo == FsmPointGraphs) { //PointGraphs
				//Debug.Log("PointGraphs");
				return GetPointGraphs(go) as PointGraph[];
			}
			return null;
		}
		
		public void SetAnything(go) {
			var typo = FsmGetTypeOf(go);	
			
			if (typo == Node) { //Node
				//Debug.Log(typo);
				return GetNode(go);
			}
			
			if (typo == Nodes.GridNode) { //GridNode
				//Debug.Log("GridNode");
				return GetGridNode(go);
			}		
			
			if (typo == List.<Node>) { //Nodes
				//Debug.Log(typo);
				return GetNodes(go);
			}
			
			if (typo == typeof(Nodes.GridNode[])) { //GridNodes
				//Debug.Log("GridNodes");
				return GetGridNodes(go);
			}	
			
			if (typo == PointGraph) { //PointGraph
				//Debug.Log(typo);
				return GetPointGraph(go);
			}
			
			if (typo == typeof(PointGraph[])) { //PointGraphs
				//Debug.Log(typo);
				return GetPointGraphs(go);
			}				
			return null;
		}


// AstarPath
		public AstarPath GetAstarPath( FsmObject go){ //Turns the action input directly into an AstarPath
			var doo = FsmAstarPath();
			doo = go.Value;
			AstarPath foo = doo.Value as AstarPath;
			return foo;
		}
		
		public FsmAstarPath SetAstarPath(AstarPath go){ //Uses FsmAstarPath as input and returns AstarData
			FsmAstarPath doo = FsmAstarPath();
			doo.Value = go;
			return doo;
		}

		
// AstarData	
		public AstarData GetAstarData(FsmObject go){ //Uses FsmAstarData as input and returns AstarData
			FsmAstarData doo = go.Value;
			AstarData foo = doo.Value;
			return foo;
		}
		public FsmAstarData SetAstarData(AstarData go){ //Uses FsmAstarData as input and returns AstarData
			FsmAstarData doo = FsmAstarData();
			doo.Value = go;
			return doo;
		}
		
		
// NavGraphs	
		public NavGraph[] GetNavGraphs(FsmObject go){ 
			FsmNavGraphs doo = go.Value;
			NavGraph[] foo = doo.Value;
			return foo;
		}
		public FsmNavGraphs SetNavGraphs(NavGraph[] go){ 
			FsmNavGraphs doo = FsmNavGraphs();
			doo.Value = go;
			return doo;
		}
		*/
// NavGraph	
		public NavGraph GetNavGraph(FsmObject go){ 
			FsmNavGraph doo = go.Value as FsmNavGraph;
			NavGraph foo = doo.Value;
			return foo;
		}
		public FsmNavGraph SetNavGraph(NavGraph go){ 
			FsmNavGraph doo = new FsmNavGraph();
			doo.Value = go;
			return doo;
		}
		/*

// PointGraphs	
		public PointGraph[] GetPointGraphs(FsmObject go){ 
			FsmPointGraphs doo = go.Value;
			PointGraph[] foo  = doo.Value;
			return foo;
		}
		public FsmPointGraphs SetPointGraphs(PointGraph[] go){ 
			FsmPointGraphs doo = FsmPointGraphs();
			doo.Value = go;
			return doo;
		}
		
// PointGraph	
		public PointGraph GetPointGraph(FsmObject go){ 
			FsmPointGraph doo = go.Value;
			PointGraph foo = doo.Value;
			return foo;
		}
		public FsmPointGraph SetPointGraph(PointGraph go){ 
			FsmPointGraph doo = FsmPointGraph();
			doo.Value = go;
			return doo;
		}
		
// node	
		public Node GetNode(FsmObject go){ 
			FsmNode doo = go.Value;
			Node foo = doo.Value;
			return foo;
		}
		public FsmNode SetNode(Node go){ 
			FsmNode doo = FsmNode();
			doo.Value = go;
			return doo;
		}
		
// nodes		
		public Node[] GetNodes(FsmObject go){ 
			FsmNodes doo = go.Value;
			Node[] foo = doo.Value;
			return foo;
		}
		public FsmNodes SetNodes(Node[] go){ 
			FsmNodes doo = FsmNodes();
			doo.Value = go;
			return doo;
		}

// gridNode	
		public Pathfinding.Nodes.GridNode GetGridNode(FsmObject go){ 
			FsmGridNode doo = go.Value;
			Pathfinding.Nodes.GridNode foo = doo.Value;
			return foo;
		}
		public FsmGridNode SetGridNode(Pathfinding.Nodes.GridNode go){ 
			FsmGridNode doo = new FsmGridNode();
			doo.Value = go;
			return doo;
		}
		
// gridNodes		
		public Pathfinding.Nodes.GridNode[] GetGridNodes(FsmObject go){ 
			FsmGridNodes doo = go.Value;
			Pathfinding.Nodes.GridNode[] foo = doo.Value;
			return foo;
		}
		public FsmGridNodes SetGridNodes(Pathfinding.Nodes.GridNode[] go){ 
			FsmGridNodes doo = FsmGridNodes();
			doo.Value = go;
			return doo;
		}

		
// path	
		public Path GetPath(FsmObject go){ 
			FsmPath doo = go.Value;
			Path foo = doo.Value;
			return foo;
		}
		public ABPath GetABPath(FsmObject go){ 
			FsmPath doo = go.Value;
			ABPath foo = doo.Value;
			return foo;
		}
		public FsmPath SetPath(Path go){ 
			FsmPath doo = FsmPath();
			doo.Value = go;
			return doo;
		}
		
// paths	
		public Path[] GetPaths(FsmObject go){ 
			FsmPaths doo = go.Value;
			Path[] foo = doo.Value;
			return foo;
		}
		public FsmPaths SetPaths(Path[] go){ 
			FsmPaths doo = FsmPaths();
			doo.Value = go;
			return doo;
		}
		
//NNConstraint
		public NNConstraint GetNNConstraint(FsmObject go){ 
			FsmNNConstraint doo = go.Value;
			NNConstraint foo = doo.Value;
			return foo;
		}
		public FsmNNConstraint SetNNConstraint(NNConstraint go){ 
			FsmNNConstraint doo = FsmNNConstraint();
			doo.Value = go;
			return doo;
		}
		
//NodeRunData
		public NodeRunData GetNodeRunData(FsmObject go){ 
			FsmNodeRunData doo = go.Value;
			NodeRunData foo = doo.Value;
			return foo;
		}
		public FsmNodeRunData SetNodeRunData(NodeRunData go){ 
			FsmNodeRunData doo = FsmNodeRunData();
			doo.Value = go;
			return doo;
		}
   	*/
	}
	}

	//public void Reset(){
	//}

	//public void OnEnter(){
	//var doo : FsmPath = InputPath.Value;
	//}
