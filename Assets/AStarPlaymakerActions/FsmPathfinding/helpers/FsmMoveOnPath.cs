using System;
using FsmPathfinding;
using Pathfinding;
using UnityEngine;

namespace HutongGames.PlayMaker.Helpers
{
	public class FsmMoveOnPath : MonoBehaviour
	{
		[Tooltip ("Stop the running temporarily. Use the set property action.")]
		public bool stop;
		
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
	  	public bool ignoreY;
	
		public float costDependendSpeed;
			
		//Add an optional offset
		public Vector3 offset;

		public Vector3 directionOut;

		//Print out debug messages.
	  	public bool LogEvents;
		
		public bool drawGizmos = true;

		private RVOController controller2;
	  	private CharacterController controller;
		private Vector3 direction;
		
	  	public int currentWaypoint = 0;
		private FsmPath doo;
		private Vector3 nextPos ;
		private float dist ;
		private float arbitraryNumber = 1/0f;
	
		public void UpdatePath()
		{
			if (InputPath == null) 
			{
				var moo = go.GetComponent<FsmMoveOnPath>();
				Destroy(moo);
			}
		}

		public void Start() 
	  	{		 	
		 	controller = go.GetComponent<CharacterController>();
			controller2 = go.GetComponent<RVOController>();
			if (controller == null && controller2 == null) 
			{
				if(AstarPath.HasPro)
				{
					controller2 = go.AddComponent<RVOController>();
					controller2.Move(new Vector3(0,0,0));
				}
				else 
				{ controller = go.AddComponent<CharacterController>(); }
			}
			UpdatePath();
      	}
		
	 	public void Update()
	 	{
			if(stop) return;
			UpdatePath();
			
			// If there is no InputPath yet.
			if (InputPath == null) { return; }
			if (currentWaypoint >= (InputPath.vectorPath).Count) 
			{
				InputPath = null;
				if (controller2 != null) //NVO controller needs to be set to 0/0/0 , else it continues running.
					controller2.Move(new Vector3(0,0,0));
				Destroy(go.GetComponent<FsmMoveOnPath>());
				return;
			}
			nextPos = InputPath.vectorPath[currentWaypoint];
			
			// Direction to the next waypoint.
			direction = (nextPos - go.transform.position).normalized;
			directionOut = direction;
			
			if (ignoreY)
			{
				direction.y = 0;
				direction = direction.normalized;
			}
			
			var multiplier = (float)((1/Math.Exp(costDependendSpeed * InputPath.path[currentWaypoint].penalty)  ) * speed * Time.fixedDeltaTime);
			direction.x *= multiplier;
			direction.y *= multiplier;
			direction.z *= multiplier;
			
			if (controller2 != null) 
			{
				controller2.Move(direction);
				controller2.maxSpeed = (float)((1/Math.Exp(costDependendSpeed * InputPath.path[currentWaypoint].penalty)  ) * speed); 
			}
			else 
			{ controller.SimpleMove(direction); }
			
			// Check if we are close enough to the next waypoint.
			dist = Vector3.Distance(go.transform.position, nextPos);
			if ( dist < nextWaypointDistance) 
			{	
				if (currentWaypoint >= (InputPath.vectorPath).Count - 1) 
				{
					if (dist >= finishDistance){return;}
				}
				// Proceed to follow the next waypoint.
				currentWaypoint++;
			}
		}
		
		public void OnDrawGizmos () 
		{
			if (InputPath.path == null || !drawGizmos) 
			{ return; }			
			
			Gizmos.color = new Color (0,1F,0,1F);
			
			if (InputPath.vectorPath != null) 
			{
				for (var i=0;i<InputPath.vectorPath.Count-1;i++) 
				{ Gizmos.DrawLine (InputPath.vectorPath[i],InputPath.vectorPath[i+1]); }
			}
		}
	}
}