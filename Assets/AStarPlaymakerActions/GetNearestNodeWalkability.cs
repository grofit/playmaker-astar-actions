using UnityEngine;
using System.Collections;
using Pathfinding;


namespace HutongGames.PlayMaker.Actions
{
   [ActionCategory("A Star")]
   [Tooltip("This is a combination of GetClosestNode and GetNodeInfo. Since you may often need to check only the walkability of a node, this can serve as a shortcut.")]
   public class GetNearestNodeWalkability   : FsmStateAction 
   {
      [RequiredField]
	  [Tooltip("Aproximate position of node. It will get the closest node to this position")]
	  public FsmVector3 Position;

		public FsmBool walkability;
		
		public FsmBool everyFrame;
	  
   
   
		public override void OnEnter() {
			mohogony();
			if (!everyFrame.Value){
			Finish();
			}
		} 
		
		void mohogony() {
			var NC = AstarPath.active.GetNearest(Position.Value);
			walkability.Value = NC.node.walkable;
		}
		
		public override void OnUpdate() {
			mohogony();
		}
	}
}