using System;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Helpers;
using FsmPathfinding;
using Pathfinding;
using System.Linq;
using UnityEngine;

namespace HutongGames.PlayMaker.Pathfinding
{
	[ActionCategory("A Star")]
	[Tooltip("Use this action to smooth a path. Right now there is no smoothing amount available yet, so avoid this on large node sizes")]
	public class smoothPath : FsmStateAction
	{
		[ActionSection("Input")]
		[Tooltip("Really any gameObject does the trick here ^^")]
		public FsmOwnerDefault gameObject;
		
		[ObjectType(typeof(FsmPath))]
		[Tooltip("Alternatively use a path directly. Overwrites everything else as a path, if set.")]
		public FsmObject InputPath;
		
		private FsmPath goo;
      
		public override void OnUpdate()
	  	{	
			goo = InputPath.Value as FsmPath;
			if(goo.Value == null) 
			{
				Finish();
				return;
			}
			
			if(FsmConverter.GetPath(InputPath).vectorPath.Count == 0) 
			{ return; } // wait until path's ready
			
			var go = gameObject.OwnerOption == OwnerDefaultOption.UseOwner ? Owner : gameObject.GameObject.Value;
			goo = InputPath.Value as FsmPath;

			var moo = (SimpleSmoothModifier)go.AddComponent(typeof(SimpleSmoothModifier));
			goo.Value.vectorPath = moo.SmoothSimple (goo.Value.vectorPath);
			
			GameObject.Destroy(moo);
			Finish();	
		}	  
   	}
}