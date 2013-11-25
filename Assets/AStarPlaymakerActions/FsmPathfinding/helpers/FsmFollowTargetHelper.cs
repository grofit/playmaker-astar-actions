using System;
using FsmPathfinding;
using Pathfinding;
using UnityEngine;

namespace HutongGames.PlayMaker.Helpers
{
	public class FsmFollowTargetHelper : MonoBehaviour
	{  		
		[Tooltip("This defines the number of frames between each path update.")]
		public int GoUpdate = 5;
		public bool drawGizmos = true;
		public int currentWaypoint = 0;
		
		[Tooltip ("Stop the running temporarily. Use the set property action.")]
		public bool stop;
		
	  	[Tooltip("Target at the time of call.")]
	  	public GameObject target;
		
		[Tooltip("Target's position at the time of call. If Target not specified this position is used. If target specified, this is the offset")]
	  	public Vector3 targetPosition;
		
		public GameObject go;   
		
		//Move along this InputPath	
		public Path InputPath;

	  	//Required movement speed.
	  	public float speed;
	  
		//Stop this distance away from your goal.
	  	public float finishDistance;
		
	  	//If the distance to the target is less than this, it's finished.
	  	public float nextWaypointDistance;
		
	  	//If the final position of the InputPath is more than this amount away from where it's supposed to be, the failure event is sent. A high value and still failure means the object can't even get close to the target.
	  	public float failureTolerance;
		
		//Moves only on the X and Z axis. Useful for walking on meshes above the grid
	  	public bool ignoreY ;
	
		[Tooltip ("Should the action finish once it reaches the target? (This would automatically call the FINISH event if there is no other active action in the state.)")]
		public bool finishOnEnd;
		
		public bool noStepBack;

		public float costDependendSpeed;

		//Add an optional offset
		public Vector3 offset;		
		
		public Vector3 directionOut;
		
		[Tooltip("The generated path")]	
		public FsmPath OutputPath;
	  	
		//Print out debug messages.
	  	public bool LogEvents;		

		public Color gizmosColor;	

	  	private CharacterController controller;
		private RVOController controller2;
		private Vector3 direction;	
			  	
		private FsmPath doo;
		private Vector3 nextPos;
		private float dist;
		private float a = 1/0f;
		private Path path;
		private Path p;
		private int pUpdate = 0;	
				
		public void CalculatePath()
		{
			var pos = (target == null) ? targetPosition : (target.transform.position + targetPosition);	
			p = ABPath.Construct (go.transform.position , pos , OnPathComplete); // create path from current position to closest/first node
			AstarPath.StartPath (p); //make the actual vector3 path which we'll use lateron.
			pUpdate = 0;
		}
			
		public void OnPathComplete(Path path) 
		{
			doo = new FsmPath(); // this needs to be an instance even if the actual path is the same.
			doo.Value = p;
			OutputPath = doo;
			currentWaypoint = 0; // new path, then it obviously has to start back at 0;
			if(noStepBack) currentWaypoint += 1;
			Update();//start Update again to avoid any problems whatsoever, especially when stopping and resuming the script.
		}
			  
		public void Start() 
	  	{
			controller = go.GetComponent<CharacterController>();
			controller2 = go.GetComponent<RVOController>();
			
			if (controller == null && controller2 == null) 
			{
				if(AstarPath.HasPro)
				{ controller2 = go.AddComponent<RVOController>(); }
				else 
				{ controller = go.AddComponent<CharacterController>(); }
			}
			
			CalculatePath();			
      	}
	  
	 	public void Update()
	 	{
			if(stop) { return; }
			if ((path == null || !(path.GetState() >= PathState.Returned)))	{ return; }
			
			if (pUpdate >= GoUpdate) 
			{ CalculatePath(); }
			
			pUpdate += 1; // count one frame on
			
			if (currentWaypoint >= (path.vectorPath).Count) 
			{
				if (finishOnEnd) 
				{
					InputPath = null;
					Destroy(go.GetComponent<FsmFollowTargetHelper>());
				}
				return;
			}			
			nextPos = path.vectorPath[currentWaypoint];
			
			// Direction to the next waypoint.
			direction = (nextPos - go.transform.position).normalized;
			directionOut = direction;
			
			if (ignoreY)
			{
				direction.y = 0;
				directionOut.y = 0;
				directionOut = directionOut.normalized;
				direction = direction.normalized;
			}
			var multiplier = (float)((1/Math.Exp(costDependendSpeed * path.path[currentWaypoint].penalty)) * speed * Time.fixedDeltaTime);
			direction.x *= multiplier;
			direction.y *= multiplier;
			direction.y *= multiplier;
			
			if (controller2 != null) 
			{
				controller2.Move(direction);
				controller2.maxSpeed = (float)((1/Math.Exp(costDependendSpeed * path.path[currentWaypoint].penalty)) * speed); 
			}
			else 
			{ controller.SimpleMove(direction); }
			
			// Check if we are close enough to the next waypoint.
			dist = Vector3.Distance(go.transform.position, nextPos);
			if ( dist < nextWaypointDistance) 
			{	
				if (currentWaypoint >= (path.vectorPath).Count - 1) 
				{
					if (dist >= finishDistance)
					{ return; }
				}
				// Proceed to follow the next waypoint.
				currentWaypoint++;
				return;
			}
		}
		
		public void OnDrawGizmos () 
		{
			if (path.path == null || !drawGizmos) {	return;	}
			Gizmos.color = gizmosColor;
			
			if (path.vectorPath != null) 
			{
				for (var i=0;i<path.vectorPath.Count-1;i++) 
				{ Gizmos.DrawLine (path.vectorPath[i],path.vectorPath[i+1]); }
			}
		}
	}

}