using HutongGames.PlayMaker.Helpers;
using FsmPathfinding;
using UnityEngine;

namespace HutongGames.PlayMaker.Pathfinding
{
	[ActionCategory("A Star")]
	[Tooltip("Use this action to smooth a path. Right now there is no smoothing amount available yet, so avoid this on large node sizes")]
	public class SmoothPath : FsmStateAction
	{
		[ActionSection("Input")]
		[Tooltip("Really any gameObject does the trick here ^^")]
		public FsmOwnerDefault gameObject;
		
		[ObjectType(typeof(FsmPath))]
		[Tooltip("Alternatively use a path directly. Overwrites everything else as a path, if set.")]
		public FsmObject InputPath;
      
		public override void OnUpdate()
	  	{	
			var fsmPath = InputPath.Value as FsmPath;
			if(fsmPath == null || fsmPath.Value == null) 
			{
				Finish();
				return;
			}
			
			if(FsmConverter.GetPath(InputPath).vectorPath.Count == 0) 
			{ return; }
			
			var go = gameObject.OwnerOption == OwnerDefaultOption.UseOwner ? Owner : gameObject.GameObject.Value;
			fsmPath = InputPath.Value as FsmPath;

			var moo = (SimpleSmoothModifier)go.AddComponent(typeof(SimpleSmoothModifier));
			fsmPath.Value.vectorPath = moo.SmoothSimple (fsmPath.Value.vectorPath);
			
			Object.Destroy(moo);
			Finish();	
		}	  
   	}
}