using System;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Helpers;
using FsmPathfinding;
using Pathfinding;
using System.Linq;

namespace HutongGames.PlayMaker.Pathfinding
{
	[ActionCategory("A Star")]
	[Tooltip("This can be used with or without an input variable and can give you a lot of detailled info on your graphs. It's one of the very lowest classes")]
	public class getAStarPathInfo : HutongGames.PlayMaker.FsmStateAction
	{
		[ActionSection("Input")]
		[ObjectType(typeof(FsmAstarPath))]
		[Tooltip("Leave this empty if you're using just one graph or if the graph is your active graph")]
		public FsmObject astarPath;		
		
		[ActionSection("Output")]		
		[ObjectType(typeof(FsmAstarData))]
		[Tooltip("AstarData saves a lot of low level stuff, just like AstarPath. It also exists per graph.")]	
		public FsmObject astarData;	
		
		[ActionSection("Booleans")]
		[Tooltip ("Set true when scanning is being done.")]
		public FsmBool isScanning;

		[Tooltip ("Shows or hides graph inspectors. ")]
		public FsmBool showGraphs;
		
		[Tooltip ("Used by the editor to show some Pro specific stuff. ")]
		public FsmBool HasPro;
		
		[Tooltip ("Returns whether or not multithreading is used.")]
		public FsmBool IsUsingMultithreading;
		
		[Tooltip ("Returns whether or not multithreading is used.")]
		public FsmBool IsAnyGraphUpdatesQueued;
		
		[ActionSection("Integers")]
		[Tooltip ("The last area index which was used. ")]
		public FsmInt lastUniqueAreaIndex;
		
		[Tooltip ("Number of threads currently active. ")]		
		public FsmInt ActiveThreadsCount;
		
		[Tooltip ("	Number of parallel pathfinders. ")]		
		public FsmInt NumParallelThreads;

		
		[ActionSection("Strings")]
		[Tooltip ("The version number for the A* Pathfinding Project. ")]		
		public FsmString Version;	

		[ActionSection("graphs...s")]
		[ObjectType(typeof(FsmNavGraphs))]
		[Tooltip ("All graphs this instance holds.This will be filled only after deserialization has completed")]		
		public FsmObject graphs;
	
		
		[ActionSection("AstarPaths")]
		[ObjectType(typeof(FsmAstarPath))]
		[Tooltip("Returns the active AstarPath object in the scene. ")]	
		public FsmObject activeAstarPath;
		
		public FsmBool everyFrame;		
		private AstarPath astarp;
		
		public override void Reset()
		{
			astarPath = null;
			astarData = null;
			isScanning = null;
			showGraphs = null;
			HasPro = null;
			IsUsingMultithreading = null;
			IsAnyGraphUpdatesQueued = null;
			lastUniqueAreaIndex = null;
			ActiveThreadsCount = null;
			NumParallelThreads = null;
			graphs = null;
			Version = null;
			activeAstarPath = null;
		}
		
		
		public override void OnEnter() 
	  	{
			DoStuff();
			
			if(!everyFrame.Value)
			{Finish();}
		} 

		public void DoStuff()
		{
			if (!astarPath.IsNone && astarPath.Value != null)
			{ astarp = FsmConverter.GetAstarPath(astarPath); }
			else
			{ astarp = AstarPath.active; }
			
			if(!isScanning.IsNone)
			{ isScanning.Value = astarp.isScanning;	}		
			
			if(!showGraphs.IsNone)
			{ showGraphs.Value = astarp.showGraphs;	}
			
			if(!IsUsingMultithreading.IsNone)
			{ IsUsingMultithreading.Value = AstarPath.IsUsingMultithreading; }	
			
			if(!IsAnyGraphUpdatesQueued.IsNone)
			{ IsAnyGraphUpdatesQueued.Value = astarp.IsAnyGraphUpdatesQueued; }			

			if(!lastUniqueAreaIndex.IsNone)
			{ lastUniqueAreaIndex.Value = astarp.lastUniqueAreaIndex; }	
			
			if(!ActiveThreadsCount.IsNone)
			{ ActiveThreadsCount.Value = AstarPath.ActiveThreadsCount; }	
			
			if(!NumParallelThreads.IsNone)
			{ NumParallelThreads.Value = AstarPath.NumParallelThreads;	}	
			
			if(!Version.IsNone)
			{ Version.Value = AstarPath.Version.ToString(); }
			
			if(!graphs.IsNone)
			{ graphs.Value = FsmConverter.SetNavGraphs(astarp.graphs); }
			
			if(!activeAstarPath.IsNone)
			{ activeAstarPath.Value = FsmConverter.SetAstarPath(AstarPath.active);	}	
			
			if (!astarData.IsNone)
			{ astarData.Value = FsmConverter.SetNavGraphs(astarp.graphs); }			
			return;
		}
	  
		public override void OnUpdate() 
	  	{
			DoStuff();
		}
   	}
	
}
