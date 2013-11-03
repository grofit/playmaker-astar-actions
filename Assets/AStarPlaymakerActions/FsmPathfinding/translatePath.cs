using System;
using FsmPathfinding;

namespace HutongGames.PlayMaker.Pathfinding
{
	[ActionCategory("A Star")]
	[Tooltip("Using the Seeker component on a GameObject a path is calculated and then followed.")]
	public class TranslatePath : FsmStateAction
	{
		[ActionSection("Input")]
		[ObjectType(typeof(FsmPath))]
		[Tooltip("Alternatively use a path directly. Overwrites everything else as a path, if set.")]	
		public FsmObject InputPath;
		
		[Tooltip("Amount of translation")]
		public FsmVector3 Vector;
			
		public override void Reset()
		{
			InputPath = null;
			Vector = null;			
		}
      
		public override void OnEnter() 
	  	{
			var inputFsmPath = InputPath.Value as FsmPath;
            if(inputFsmPath == null)
            { throw new NullReferenceException("Input Path does not contain a valid path"); }

			var path = inputFsmPath.Value;
			if(path == null) 
			{
				Finish(); 
				return;
			}
			
			var x = 0;
			while (x < path.path.Count)
			{
				path.vectorPath[x] += Vector.Value;
				x++;
			}
			Finish();			
		}
   	}
}