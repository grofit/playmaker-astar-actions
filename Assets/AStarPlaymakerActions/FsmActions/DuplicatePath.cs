using System;
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
			var inputFsmPath = InputPath.Value as FsmPath;
			if( (inputFsmPath == null) || (inputFsmPath.Value == null) || !OutputPath.UseVariable) 
			{
				Debug.Log("Input Incomplete"); 
				Finish(); 
				return;
			}				
			
			var underlyingABPath = (InputPath.Value as FsmPath).Value as ABPath;	
		    if(underlyingABPath == null)
            { throw new NullReferenceException("There is no underlying path in the input path"); }

			var newAbPath = PathPool<ABPath>.GetPath();
            newAbPath.duration = underlyingABPath.duration;
            newAbPath.heuristicScale = underlyingABPath.heuristicScale;
            newAbPath.enabledTags = underlyingABPath.enabledTags;
            newAbPath.radius = underlyingABPath.radius;
            newAbPath.searchedNodes = underlyingABPath.searchedNodes;
            newAbPath.searchIterations = underlyingABPath.searchIterations;
            newAbPath.speed = underlyingABPath.speed;
            newAbPath.turnRadius = underlyingABPath.turnRadius;
            newAbPath.recycled = underlyingABPath.recycled;
            newAbPath.nnConstraint = underlyingABPath.nnConstraint;
            newAbPath.path = underlyingABPath.path;
            newAbPath.vectorPath = underlyingABPath.vectorPath;

            OutputPath.Value = new FsmPath { Value = newAbPath };
			Finish();	
		}
   	}
}