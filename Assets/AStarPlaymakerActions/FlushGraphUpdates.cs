namespace HutongGames.PlayMaker.Actions
{
   [ActionCategory("A Star")]
   [Tooltip("Forces graph updates to run. This will force all graph updates to run immidiately. Or rather, it will block the Unity main thread until graph updates can be performed and then issue them. This will force the pathfinding threads to finish calculate the path they are currently calculating (if any) and then pause. When all threads have paused, graph updates will be performed. ")]
   public class FlushGraphUpdates : FsmStateAction 
   {
		public override void OnEnter() 
        {
			AstarPath.active.FlushGraphUpdates();
			Finish();
		} 
	}
}