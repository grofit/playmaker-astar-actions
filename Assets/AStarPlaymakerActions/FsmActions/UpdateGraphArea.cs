using UnityEngine;

namespace HutongGames.PlayMaker.Pathfinding
{
   [ActionCategory("A Star")]
   [Tooltip("Updates a square area")]
   public class UpdateGraphArea   : FsmStateAction 
   {   
        [Tooltip("Center/Origin in worldspace.")]
        public FsmVector3 center;
        
        [Tooltip("Size in Worldspace")]
        public FsmVector3 size;
       
		public override void OnEnter() 
        {
			var bounds = new Bounds (center.Value, size.Value);
			AstarPath.active.UpdateGraphs(bounds);
			Finish();
		} 
	}
}