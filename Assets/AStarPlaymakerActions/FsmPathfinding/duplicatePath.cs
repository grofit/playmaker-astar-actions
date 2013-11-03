using HutongGames.PlayMaker.Helpers;
using FsmPathfinding;
using Pathfinding;
using UnityEngine;

namespace HutongGames.PlayMaker.Pathfinding
{
	[ActionCategory("A Star")]
	[Tooltip("This action creates an exact replica of the first path, without them being related to each other.")]
	public class DuplicatePath : FsmStateAction
	{
		[ActionSection("Input")]
		[ObjectType(typeof(FsmPath))]
		[Tooltip("Input Path")]
		public FsmObject InputPath;
		
		[ActionSection("Output")]
		[ObjectType(typeof(FsmPath))]
		[Tooltip("Save duplicate path to this")]
		public FsmObject OutputPath;
				
		public override void Reset()
		{
			InputPath = new FsmObject();
			OutputPath = new FsmObject();			
		}
      
		public override void OnEnter() 
	  	{
			var mo = InputPath.Value as FsmPath;
			if( (mo == null) || (mo.Value == null) || !OutputPath.UseVariable) 
			{
				Debug.Log("Input Incomplete"); 
				Finish(); 
				return;
			} // also abort the action if there is no variable to save to.					
			
			var a = (InputPath.Value as FsmPath).Value as ABPath;			
			var b = PathPool<ABPath>.GetPath(); // I can't instantiate so there's nothing but the manual way left
			
			b.duration = a.duration;
			b.heuristicScale = a.heuristicScale;
			b.enabledTags = a.enabledTags;
			b.radius = a.radius;
			b.searchedNodes = a.searchedNodes;
			b.searchIterations = a.searchIterations;
			b.speed = a.speed;
			b.turnRadius = a.turnRadius;
			b.recycled = a.recycled;
			b.nnConstraint = a.nnConstraint;
			b.path = a.path;
			b.vectorPath = a.vectorPath;
			
			OutputPath.Value = FsmConverter.SetPath(b);
			Finish();	
		}

   	}
}