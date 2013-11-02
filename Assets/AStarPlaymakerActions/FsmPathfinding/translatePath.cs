using System;
using HutongGames.PlayMaker;
using FsmPathfinding;
using Pathfinding;
using System.Linq;

namespace HutongGames.PlayMaker.Pathfinding
{
	[ActionCategory("A Star")]
	[Tooltip("Using the Seeker component on a GameObject a path is calculated and then followed.")]
	public class translatePath : FsmStateAction
	{
		[ActionSection("Input")]
		[ObjectType(typeof(FsmPath))]
		[Tooltip("Alternatively use a path directly. Overwrites everything else as a path, if set.")]	
		public FsmObject InputPath;
		
		[Tooltip("Amount of translation")]
		public FsmVector3 Vector;
		
		private Path a;
		
		public void Reset()
		{
			InputPath = null;
			Vector = null;			
		}
      
		public override void OnEnter() 
	  	{
			var doo = InputPath.Value as FsmPath;
			a = doo.Value;
			
			if(a == null) 
			{
				Finish(); 
				return;
			}
			
			var x = 0;
			while (x < a.path.Count)
			{
				//a.path[x].position.x += Vector.Value.x/a.path[x].position.PrecisionFactor;
				//a.path[x].position.y += Vector.Value.y/a.path[x].position.PrecisionFactor;
				//a.path[x].position.z += Vector.Value.z/a.path[x].position.PrecisionFactor;
				a.vectorPath[x] += Vector.Value;
				x++;
			}
			Finish();			
		}
   	}
}