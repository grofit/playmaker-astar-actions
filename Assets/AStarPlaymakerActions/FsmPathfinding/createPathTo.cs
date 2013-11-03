using FsmPathfinding;
using Pathfinding;
using UnityEngine;

namespace HutongGames.PlayMaker.Pathfinding
{
	[ActionCategory("A Star")]
	[Tooltip("Using the Seeker component on a GameObject a path is calculated and then followed.")]
	public class CreatePathTo : FsmStateAction
	{
		[Tooltip("Uses this as the start Position. Also requires a seeker component to create the path. You can create one before calling this action and remove it right after this action is done if you want / if the gameObject is some generic thing.")]
      	public FsmOwnerDefault gameObject;     
		
		[Tooltip("Target at the time of call.")]
	  	public FsmGameObject target;
		
		[Tooltip("Target's position at the time of call. If Target not specified this position is used.")]
	  	public FsmVector3 targetPosition;
		
		[Tooltip("This is sent once the path is finished")]
	  	public FsmEvent PathComplete;

		[ActionSection("Debug")]	  	
		[Tooltip("Print out debug messages.")]
	  	public FsmBool LogEvents;
		
		[ActionSection("Output")]
		[ObjectType(typeof(FsmPath))]
		[Tooltip("Alternatively use a path directly. Overwrites everything else as a path, if set.")]	
		public FsmObject OutputPath;
     
    	public override void Reset() 
	  	{
         	gameObject = null;
			target = new FsmGameObject();
			targetPosition = null;
			PathComplete = null;
			OutputPath = null;
      	}
				
		public override void OnEnter() 
	  	{
		 	var targetGameObject = gameObject.OwnerOption == OwnerDefaultOption.UseOwner ? Owner : gameObject.GameObject.Value;

			if(!OutputPath.UseVariable) 
			{
				Debug.Log("create Path : No variable to save path to"); 
				Finish(); 
				return;
			}
			
			if (target.Value != null)
			{
				targetPosition = target.Value.transform.position;
				
				if (LogEvents.Value)
				{ Debug.Log ("Target was specified, getting position."); }
			}

            CalculatePath(targetGameObject);
      	}		
		
	  	public void CalculatePath(GameObject targetGameObject) 
		{
			var path = ABPath.Construct(targetGameObject.transform.position , targetPosition.Value , OnPathComplete);
			AstarPath.StartPath(path);

	  	    if (LogEvents.Value)
	  	    { Debug.Log(string.Format("Start Position {0} End Position {1}", targetGameObject.transform.position, targetPosition.Value)); }
	  	}
		
	  	public void OnPathComplete(Path path) 
	  	{
			if (LogEvents.Value)
			{ Debug.Log ("Path Completed"); }
			
			var fsmPath = new FsmPath { Value = path };
	  	    OutputPath.Value = fsmPath;
			
			Debug.Log((OutputPath.Value as FsmPath).Value);

			if (PathComplete != null) 
			{ Fsm.Event(PathComplete); }
			else
			{ Finish(); }		
		}	  
   	}	
}
