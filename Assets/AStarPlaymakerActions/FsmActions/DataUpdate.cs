namespace HutongGames.PlayMaker.Pathfinding
{
   [ActionCategory("A Star")]
   [Tooltip("Update the underlying AStarPath data")]
   public class DataUpdate : FsmStateAction 
   {   
	    public override void OnEnter() 
        {
			AstarPath.active.DataUpdate();
			Finish();
		} 
	}
}