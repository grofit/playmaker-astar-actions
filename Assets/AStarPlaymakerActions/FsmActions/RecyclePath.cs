using HutongGames.PlayMaker.Pathfinding;

namespace HutongGames.PlayMaker.Pathfinding
{
	[ActionCategory("A Star")]
	[Tooltip("Resets and returns a path to the path pool")]
	public class RecyclePath : FsmStateAction
	{
		[ActionSection("Input")]
		[ObjectType(typeof(FsmPath))]
		[Tooltip("Input Path")	]
		public FsmObject InputPath;
		
		public override void Reset()
		{
			InputPath = null;			
		}

		public override void OnEnter() 
	  	{
			var fsmPath = InputPath.Value as FsmPath;
			if(fsmPath.Value == null) 
			{
				Finish(); 
				return;
			}
			throw new System.Exception ("This function should not be used directly. Use path.Release (...) and path.Claim (...) instead. Such an integration is coming soon.");
		}
   	} 
}