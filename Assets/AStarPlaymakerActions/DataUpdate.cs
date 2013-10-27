using UnityEngine;
using System.Collections;
using Pathfinding;


namespace HutongGames.PlayMaker.Actions
{
   [ActionCategory("A Star")]
   [Tooltip("")]
   public class DataUpdate   : FsmStateAction 
   {   
	  
   
   
		public override void OnEnter() {
			AstarPath.active.DataUpdate();
			Finish();
		} 
	}
}