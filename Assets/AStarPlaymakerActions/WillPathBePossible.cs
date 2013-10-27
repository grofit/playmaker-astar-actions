using UnityEngine;
using System.Collections;
using Pathfinding;



namespace HutongGames.PlayMaker.Actions
{
   [ActionCategory("A Star")]
   [Tooltip("This action finds out if, after using the updateGraph action (or similar), there will or will not be a possible path between the start position and the end position. It does not actually update the graph.")]
   public class WillPathBePossible   : FsmStateAction 
   {
      [RequiredField]
	  [Tooltip("Object that needs to be added to the graph")]
	  [CheckForComponent(typeof(Collider))]
      public FsmOwnerDefault target;    
	  
	  [Tooltip("Start of the path")]
	  public FsmVector3 Start;

	  [Tooltip("End pf the path")]
	  public FsmVector3 End;
	  
	  public FsmBool PathIsPossible;
	  
	  public FsmBool everyFrame;
	  

		private GraphUpdateObject guo;
		private Node startNode;
		private Node endNode;
	  	public override void Reset()
		{
		}
   
		public void DoStuff() {
			guo = new GraphUpdateObject(Fsm.GetOwnerDefaultTarget(target).collider.bounds);
			startNode = AstarPath.active.GetNearest (Start.Value).node;
			endNode = AstarPath.active.GetNearest (End.Value).node;
			PathIsPossible.Value = GraphUpdateUtilities.UpdateGraphsNoBlock(guo, startNode, endNode, true);
		}
   
		public override void OnEnter() {
			DoStuff();
			if (!everyFrame.Value) { Finish(); }
		} 
		public override  void OnUpdate() {
			DoStuff();
			if (!everyFrame.Value) { Finish(); }
		}
	}
}