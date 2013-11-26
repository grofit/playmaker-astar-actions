namespace HutongGames.PlayMaker.Pathfinding
{
   [ActionCategory("A Star")]
   [Tooltip("Scans all graphs")]
   public class ScanGraphs : FsmStateAction 
   {
		public override void OnEnter() 
        {
			AstarPath.active.Scan();
			Finish();
		} 
	}
}