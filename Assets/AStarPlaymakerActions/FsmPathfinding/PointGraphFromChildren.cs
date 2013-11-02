using System;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Helpers;
using FsmPathfinding;
using Pathfinding;
using System.Linq;
using AstarPathExtension;
using UnityEngine;

namespace HutongGames.PlayMaker.Pathfinding
{
	[ActionCategory("A Star")]
	[Tooltip("recursively checks all children of a gameObject and adds their positions to a PointGraph. This creates a pathlike or gridlike graph, depending on the width. For other irregular types of graphs, check ComplexPointGraphFromChildren. The advantage of this is that you only need to pose 2 empties to define a long path, but you're still able to make it more detailled at certain parts. The complex version will create node connections based on the distance between the empties, so if you set the minimal distance needed for a connection to something very high to accomplish a connection between 2 nodes that are very far from each other, then you won't be able to make a smooth curve right after the second node because instead of connecting the second node to the third, it will connect it to the last or even further off. So that's why there's 2 of these actions ;) ")]
	public class PointGraphFromChildren : HutongGames.PlayMaker.FsmStateAction
	{
		[ActionSection("Input")]
		[RequiredField]
		[ObjectType(typeof(FsmPointGraph))]
		[Tooltip("The point graph")]
		public FsmObject graph;
		
		[Tooltip("You can give this graph whatever name you want. If you want to get it by its' name lateron thouh, make sure that the name is unique.")	]
      	public FsmString name;
			
		[RequiredField]
		[Tooltip("The root of the gameObjects you want to use as a graph")]
      	public FsmOwnerDefault gameObject;
		
		[Tooltip("How easy are the nodes to walk over? If it's a path through the swamp, then you may want to turn this up. Movement actions also have the option to move more slowly on nodes with high cost, and astar generally avoids them unless the cost is less than the cost of the additional nodes needed to go around them (in this case going around the swamp) would take.")	]
		public FsmInt cost;
		
		[Tooltip("How wide should the path be? 0 and 1 go like this ._._._._. (the point being the node, the line being the connection). 2 Would go like this  :=:=:=:=: (with additional connections between the nodes that make up a column :D ) Each node can only have 4 connections at the most. ")	]
		public FsmInt width {get;set;}
				
		
		[ActionSection ("Output : Nodes...s")]
		[Tooltip("If you're using a grid graph, this will be what you need.")]
		[ObjectType(typeof(FsmNodes))]
		public FsmObject Nodes;
		
		[Tooltip("If true, this always creates a new PointGraph. If False, this adds to the current PointGraph in the graph variable.")]
		public FsmBool alwaysNew;
		
		[Tooltip("Gets the closest node to the first child gameObject and connects the newly created node with that already existing node. Needed to connect the new graph to an old one.")]
		public FsmBool connectStart;
		
		[Tooltip("Gets the closest node to the last child gameObject and connects the newly created node with that already existing node. Needed to connect the new graph to an old one.")]
		public FsmBool connectEnd;
		
		private PointGraph g;
		private NNConstraint nnc;
		private FsmPointGraph mo;
	  
		public override void Reset()
		{
			graph = null;
		}
		
		public override void OnEnter() 
	  	{
			mo = graph.Value as FsmPointGraph;
			//Debug.Log(typeof(mo));
			if ((mo == null) ||(mo.Value == null) || alwaysNew.Value) 
			{
				AstarPath.active.astarData.AddGraph(mo.Value);
				g = FsmConverter.GetPointGraph(graph) as PointGraph;
				Debug.Log ("Creating New Point Graph");
				
				graph.Value = FsmConverter.SetPointGraph(g);
			} // create a PointGraph if the variable does not contain a valid one already.
			else 
			{ g = FsmConverter.GetPointGraph(graph) as PointGraph; }
			
			DoStuff();
			Finish();			
		}
		
		public void DoStuff()
		{
		 	var go = gameObject.OwnerOption == OwnerDefaultOption.UseOwner ? Owner : gameObject.GameObject.Value;
		 	
		 	g.root = go.transform; // set the root so the scan will turn it into nodes
		 	g.maxDistance = -1; // no autoconnect
			g.initialPenalty = (uint)cost.Value;
			g.name = name.Value;
		 	AstarPathExtensions.ScanGraph (g); // turn the gameObjects into ndoes.
			
			if(width.Value <= 1)
			{
				for (var i =1; i<g.nodes.Length; i++)
				{
					 // connect the node with the previously created node
					g.nodes[i].AddConnection(g.nodes[i-1], cost.Value);
					g.nodes[i-1].AddConnection(g.nodes[i], cost.Value);	
				}
	
				if (connectStart.Value || connectEnd.Value)
				{
					// You would want to use an NNConstraint to ignore this graph when searching (graphMask)
					// Since it currently will find g.nodes[0] when searching
					nnc = NNConstraint.Default;
					var nncSave = nnc.graphMask;
					var index = AstarPath.active.astarData.GetGraphIndex(g);
					nnc.graphMask = ~(1 << index);			

					if (connectStart.Value) {connectEm(0);}	
					if (connectEnd.Value) {connectEm(g.nodes.Length - 1);}
					nnc.graphMask = nncSave;
				}
			}
			else
			{
				for (var i =0; i<g.nodes.Length; i++)
				{
					 // connect the node with the previously created node
					//g.nodes[i].AddConnection(g.nodes[i-1], cost.Value);
					//g.nodes[i-1].AddConnection(g.nodes[i], cost.Value);
					//if(i + width.Value <= g.nodes.Length)
					//{
						 //connect the node with the next node in line
						//g.nodes[i-1].AddConnection(g.nodes[i-1+width.Value], cost.Value);
						//g.nodes[i-1+width.Value].AddConnection(g.nodes[i-1], cost.Value);
					//}
					
					// there are 3 scenarios: Either a node is in the middle or at either of the ends of the row.
					if(i % width.Value == width.Value - 1 ) //2
					{
						if(i+width.Value <g.nodes.Length)
						{
							g.nodes[i].AddConnection(g.nodes[i+width.Value], cost.Value);
							g.nodes[i+width.Value].AddConnection(g.nodes[i], cost.Value);
						}
							
						if(i+width.Value -1 <g.nodes.Length) 
						{
							g.nodes[i].AddConnection(g.nodes[i+width.Value-1], cost.Value);
							g.nodes[i + width.Value-1].AddConnection(g.nodes[i], cost.Value);
						}						
					}
					else if(i ==0 ||  0 == i%width.Value)//1 // if i is 0 or a multiple of width
					{
						if(i+width.Value <g.nodes.Length)
						{
							g.nodes[i].AddConnection(g.nodes[i+width.Value], cost.Value);
							g.nodes[i+width.Value].AddConnection(g.nodes[i], cost.Value);
						}
						if(i+1 <g.nodes.Length)
						{
							g.nodes[i].AddConnection(g.nodes[i+1], cost.Value);
							g.nodes[i + 1].AddConnection(g.nodes[i], cost.Value);
						}
						if(i+width.Value+1 <g.nodes.Length)
						{
							g.nodes[i].AddConnection(g.nodes[i+width.Value+1], cost.Value);
							g.nodes[i + width.Value + 1].AddConnection(g.nodes[i], cost.Value);
						}
					}
					else //3
					{
						if(i + width.Value <g.nodes.Length)
						{
							g.nodes[i].AddConnection(g.nodes[i+width.Value], cost.Value);
							g.nodes[i+width.Value].AddConnection(g.nodes[i], cost.Value);
						}
						
						if(i+1 <g.nodes.Length)
						{
							g.nodes[i].AddConnection(g.nodes[i+1], cost.Value);
							g.nodes[i + 1].AddConnection(g.nodes[i], cost.Value);
							}
						
						if(i+width.Value+1 <g.nodes.Length)
						{
							g.nodes[i].AddConnection(g.nodes[i+width.Value+1], cost.Value);
							g.nodes[i + width.Value + 1].AddConnection(g.nodes[i], cost.Value);
						}
						
						if(i+width.Value -1 <g.nodes.Length) 
						{
							g.nodes[i].AddConnection(g.nodes[i+width.Value-1], cost.Value);
							g.nodes[i + width.Value - 1].AddConnection(g.nodes[i], cost.Value);
						}
					}					
				}
	
				if (connectStart.Value || connectEnd.Value)
				{
					// You would want to use an NNConstraint to ignore this graph when searching (graphMask)
					// Since it currently will find g.nodes[0] when searching
					nnc = NNConstraint.Default;
					var nncSave = nnc.graphMask;
					var index = AstarPath.active.astarData.GetGraphIndex(g);
					nnc.graphMask = ~(1 << index);			

					if (connectStart.Value) {connectEm(0);}					
					if (connectEnd.Value) {connectEm(g.nodes.Length - width.Value);}	

					nnc.graphMask = nncSave;
				}			
			}
			
			Nodes.Value = FsmConverter.SetNodes(FsmConverter.NodeListToArray(g.nodes));

			//Required since the graph connections have been updated
			AstarPath.active.FloodFill ();
			
			//g.AddChildren(Count, go.transform);
			return;
		}
		
		public void connectEm(int Count) 
		{
			for(var i = 0; i< width.Value || i == 0; i++)
			{
				var pos = g.nodes[Count + i].position;
				var nodeinf = AstarPath.active.GetNearest(new Vector3 (pos.x,pos.y,pos.z)*Int3.PrecisionFactor, nnc);
				if (nodeinf.node != null) {
					nodeinf.node.AddConnection(g.nodes[Count + i], cost.Value);
					g.nodes[Count + i].AddConnection(nodeinf.node, cost.Value);
				}
			}
		}	  
   	}	
}