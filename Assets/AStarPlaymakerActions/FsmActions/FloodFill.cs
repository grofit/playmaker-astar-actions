namespace HutongGames.PlayMaker.Pathfinding
{
   [ActionCategory("A Star")]
   [Tooltip("Floodfills all graphs and updates areas for every node. ")]
   public class FloodFill : FsmStateAction 
   {   
	    public override void OnEnter() 
        {
			AstarPath.active.FloodFill();
			Finish();
		} 
	}
}