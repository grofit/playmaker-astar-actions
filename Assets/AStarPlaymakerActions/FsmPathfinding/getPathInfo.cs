using FsmPathfinding;
using UnityEngine;

namespace HutongGames.PlayMaker.Pathfinding
{
	[ActionCategory("A Star")]
	[Tooltip("get various properties of the path. Get properties does not work due to the wrappers.")]
	public class GetPathInfo : FsmStateAction
	{
		[ActionSection("Input")]
		[ObjectType(typeof(FsmPath))]
		[Tooltip("Alternatively use a path directly. Overwrites everything else as a path, if set.")]	
		public FsmObject InputPath;
		
		[ActionSection("Output - Floats")]
		[Tooltip("The real length of the path")]	
		public FsmFloat Length;
		
		[Tooltip("The duration of this path in ms. ")]	
		public FsmFloat duration;
		
		[Tooltip("Scale of the heuristic values. ")]	
		public FsmFloat heuristicScale;
		
		[ActionSection("Integers")]
		[Tooltip("Which graph tags are traversable. ")]
		public FsmInt enabledTags;
		
		[Tooltip("Radius for the unit searching for the path. Not used by any built-in pathfinders. These common name variables are put here because it is a lot faster to access fields than, for example make use of a lookup table (e.g dictionary). Or having to cast to another path type for acess. ")]
		public FsmInt radius;
		
		[Tooltip("ID of this path. ")]
		public FsmInt pathID;
		
		[Tooltip("Number of nodes this path has searched. ")]
		public FsmInt searchedNodes;
		
		[Tooltip("ID of this path. ")]
		public FsmInt searchIterations;
		
		[Tooltip("Speed of the character. ")]
		public FsmInt speed;
		
		[Tooltip("Turning radius of the character. ")]
		public FsmInt turnRadius;		
		
		[ActionSection("Bools")]
		[Tooltip("Calculate partial path if the target node cannot be reached. ")	]
		public FsmBool IsDone;

		[Tooltip("True if the path is currently recycled (i.e in the path pool). ")]	
		public FsmBool recycled;
		
		[ActionSection("NNConstraints")]
		[ObjectType(typeof(FsmNNConstraint))]
		[Tooltip("Constraint for how to search for nodes. ")	]
		public FsmObject nnConstraint;

		[ActionSection("Nodes...s")]
		[ObjectType(typeof(FsmNodes))]
		[Tooltip("Constraint for how to search for nodes. ")	]
		public FsmObject nodes;

		[ActionSection("NodeRunData")]
		[ObjectType(typeof(FsmNodeRunData))]
		[Tooltip("Constraint for how to search for nodes. ")	]
		public FsmObject runData;
		
		public FsmBool everyFrame;
		
		private FsmPath goo = new FsmPath();	
		
		
		public override void Reset() 
		{
			InputPath = new FsmObject();
			Length = new FsmFloat();
			duration = new FsmFloat();
			heuristicScale = new FsmFloat();			
			enabledTags = new FsmInt();
			radius = new FsmInt();
			pathID = new FsmInt();
			searchedNodes = new FsmInt();
			searchIterations = new FsmInt();
			speed = new FsmInt();
			turnRadius = new FsmInt();
			IsDone = new FsmBool();
			recycled = new FsmBool();
			nnConstraint = new FsmObject();
			nodes = new FsmObject();
			runData = new FsmObject();
		}
		
		public override void OnEnter() 
	  	{
			if(InputPath.Value == null || InputPath.Value is UnityEngine.Object || (InputPath.Value as FsmPath).Value == null) 
			{
				Debug.Log("Input Path is invalid. Make sure that the path is completely created (many actions do not finish in the same frame as they start) ");
				Finish(); 
				return;
			}
			DoStuff();
			if(!everyFrame.Value)
			{ Finish(); }
			
		}

		public void DoStuff()
		{
			var doo = InputPath.Value as FsmPath;
			
			if(doo.Value == null)
			{
				Finish(); 
				return;
			}
			
			//floats
			Length.Value = doo.Value.GetTotalLength();
			duration.Value = doo.Value.duration;
			heuristicScale.Value = doo.Value.heuristicScale;
			
			//ints
			enabledTags.Value = doo.Value.enabledTags;			
			radius.Value = doo.Value.radius;
			pathID.Value = doo.Value.pathID;
			searchedNodes.Value = doo.Value.searchedNodes;
			searchIterations.Value = doo.Value.searchIterations;
			speed.Value = doo.Value.speed;
			turnRadius.Value = doo.Value.turnRadius;

			//bools
			IsDone.Value = doo.Value.IsDone ();
			recycled.Value = doo.Value.recycled;
			
			
			//NNConstraints
			var noo = new FsmNNConstraint();
			noo.Value = doo.Value.nnConstraint;
			nnConstraint.Value = noo;
			
			//Nodes[]
			var coo = new FsmNodes();
			coo.Value = doo.Value.path;
			nodes.Value = coo;
			
			//NodeRunData
			var roo = new FsmNodeRunData();
			roo.Value = doo.Value.runData;
			runData.Value = roo;
			
		}
	  
		public override void OnUpdate() 
	  	{
			DoStuff();
		}
   	}
}