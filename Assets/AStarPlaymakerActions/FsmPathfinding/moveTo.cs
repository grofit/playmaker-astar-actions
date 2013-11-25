using System;
using HutongGames.PlayMaker.Helpers;
using FsmPathfinding;
using HutongGames.PlayMaker.Pathfinding.Enums;
using Pathfinding;
using UnityEngine;

namespace HutongGames.PlayMaker.Pathfinding
{
    [ActionCategory("A Star")]
	[Tooltip("move an object from its' current position directly to the target position or gameObject")]
	public class MoveTo : FsmStateAction
	{
		[Tooltip("What kind of movement do you want? Depending on the mode new settings will appear. Move To moves to the position of the target at the time of call. Follow To updates until it reaches the target, then finishes. Follow keeps following indefinitely. And Follow Path uses a path input.")]
		public MoveMode moveMode;
		
		[Tooltip("The length of the path in GScore(pro users : just multiply your unity distance with the distance between each node(the node size on grid graphs) ) to flee/randomly walk on. If you use flee continuously, make sure to turn this to as low as possible, to save performance. All features that involve this attribute are noticably faster in the pro version.")]
		public FsmFloat length = 10f;
		
		[Tooltip("How many frames should the script wait between updating the path towards the target.")]
		public FsmInt updateInterval = 0;
		
		[Tooltip("Updates the shadowPath if the target or targetPosition moved more than this distance from the endwaypoint of the current path. If you're using a gridgraph, you might want to set this to the same thing as the node size :) ")]
		public FsmFloat shadowUpdateDistance  = 1f;

		[Tooltip("Uses this as the start Position and moves this gameObject")]
      	public FsmOwnerDefault actor; 
		
		[ObjectType(typeof(FsmPath))]
		[Tooltip("If the path is unequal null , move along this path. Else use the target or target position")]
		public FsmObject inputPath;
		
		[Tooltip("should changes on the inputPath update the path this action takes?")]
		public bool updatePath = false;
		
	  	[Tooltip("Target at the time of call.")]
	  	public FsmGameObject target;
		
		[Tooltip("Target's position at the time of call. If Target not specified this position is used. If target specified, this is the offset")]
	  	public FsmVector3 targetPosition;
		
		[Tooltip("What type of controller would you like to use? The RVO controller is a pro only feature. If you choose <available> it will use whatever is already on your actor. If you choose none, you can use the direction output to controle it yourself")]
		public ControllerType controllerType = ControllerType.available;
		
	  	[Tooltip("Required movement speed.")]
	  	public FsmFloat speed = 1f;
		
		[Tooltip("Should the actor move exactly along the path or should it smoothly interpolate from one node to the next? The radius of each turn depends on the turnRadius")]
		public FsmBool smoothTurns; 
		
		[Tooltip("turnradius in worldunits")]
		public FsmFloat turnRadius = new FsmFloat{Value = 0.5f};
	  
	  	[Tooltip("This is sent once the actor arrives at the target.")]
	  	public FsmEvent endOfPathEvent;
	  
	  	[Tooltip("The event that is sent when the path is unreachable.")]
	  	public FsmEvent failedEvent;
	  
	  	[Tooltip("If the distance to the target is less than this, it's finished.")]
	  	public FsmFloat nextWaypointDistance = 3f;
		
		[Tooltip("if set to absolute, the action will send the finished event once the distance between the actor and the target is less than finishDistance. Using absoluteEndnode it will send Finish once the distance between the last node on the path and the Actor is less than finishDistance. If set to relative, it will send the finish event if the distance along the path is less than finishDistance. If set to last it will go to the second to last node on the path and only then starts checking whether the distance to the last node is less than the finishDistance. Use last on simple movement actions, as it is by far the cheapest. Use relative only when you must, as it can be a tad expensive if your finishDistance is very high.")]
		public FinishDistance finishDistanceMode = FinishDistance.absolute;
		
		[Tooltip("Stop this distance away from your goal.")]
	  	public FsmFloat finishDistance = 1f;
	  
		[Tooltip("Turn this on to add the target position (or the target object position, if it exists) as the last waypoint. Use this if your target is an empty or a position on the floor to move precisely to that position (and not to the closest node nearby)")]
		public FsmBool exactFinish;
	  
	  	[Tooltip("If the final position of the path is more than this amount away from where it's supposed to be, the failure event is sent. A high value and still failure means the object can't even get close to the target.")]
	  	public FsmFloat failureTolerance = 10f;
		
		[Tooltip("Moves only on the X and Z axis. Useful for walking on meshes above the grid")]
	  	public FsmBool ignoreY;
		
		[Tooltip("Should the actor follow even if the action or state is no longer active? If true, this also immediatly fires the endOfPathEvent")]
		public FsmBool auto;
		
		[Tooltip("set this to a positive value to make the actor walk more slowly if it's walking on a node that has a cost value assigned to it. Set to 0 to turn it off.")]
		public FsmFloat costDependendSpeed;
		
		[Tooltip("If true, starts at the first node of the path. If false, starts at the closest node. Not needed if the state is picked up in the same or close to the same position you left it when first using the action.")]
		public FsmBool startAtStart;
	
		[Tooltip("should the action calculate addidtional nodes to connect the current position to the first/nearest node of the path?")]
		public FsmBool connectPath;
		
		[ActionSection("Output")]
		public FsmVector3 directionOut;
		
		[ObjectType(typeof(FsmPath))]
		[Tooltip("The generated path")]	
		public FsmObject OutputPath;		
		
		[Tooltip("The real speed of the actor, with the node costs taken into account")]
		public FsmFloat outputSpeed;
		
		[ActionSection("Debug")]
		[Tooltip("Print out debug messages.")]
	  	public FsmBool LogEvents;
		
		public bool drawGizmos = true;
		
		public FsmColor gizmosColor;

        private Path path; 
      	private GameObject targetGameObject;
		
		private RVOController controller2;
	  	private CharacterController controller;
		private Rigidbody rigidbody ;
	  //	private var seeker : Seeker;
		private Vector3 direction;
		
		private Vector3 prevPosition;
		private Vector3 prevTarget;
		
	  	private int currentWaypoint = 1;
		private FsmPath newFsmPath;
		private Vector3 nextPos;
		private float distance;
		private float a = 1/0f;
		private Path abPath;
		private int frame = 0;
		private Vector3 nextDirection;
		private bool firstFrame = true;
		private bool pathLoading = false; // this is true whenever the follow mode already waits for the next path. This is important whenever the path creation takes more frames than what the update interval is.
		
		private float time = 0f;
		
     	public override void Reset() 
	  	{
         	actor = null;
			inputPath = null;
			target = null;
			targetPosition = null;
			speed = 200f;
			endOfPathEvent = null;
			failedEvent = null;
			nextWaypointDistance = 1f;
			finishDistance = 1f;
			failureTolerance = 3f;
			length = 10f;
			ignoreY = true;
			auto = false;
			directionOut = null;
			OutputPath = null;
			costDependendSpeed = 0f;
      	}
		
		public override void OnEnter() 
	  	{
			if(moveMode == MoveMode.followPath)
			{
				path = inputPath.GetPath();
				if(path == null)
				{
					if(LogEvents.Value)
					{ Debug.Log("Astar Follow Path failed. The path is null"); }
					
					Fsm.Event(failedEvent); 
					Finish(); 
					return;
				}
			
                if(path.vectorPath.Count == 0)
			    {
			        if(LogEvents.Value)
			        { Debug.Log("Astar Follow Path failed. The path contains no nodes"); }

			        Fsm.Event(failedEvent); 
			        Finish(); 
			        return;				
			    }
			}
			
			currentWaypoint = 0;
		 	targetGameObject = actor.OwnerOption == OwnerDefaultOption.UseOwner ? Owner : actor.GameObject.Value;
			if( targetGameObject == null) 
			{
				if(LogEvents.Value)
				{ Debug.Log("Astar Move To failed. The actor is null"); }
				
				Fsm.Event(failedEvent); 
				Finish(); 
				return;
			}
			
		 	controller = targetGameObject.GetComponent<CharacterController>();
			controller2 = targetGameObject.GetComponent<RVOController>();
			rigidbody = targetGameObject.GetComponent<Rigidbody>();
			
			if(controllerType == ControllerType.characterController && controller == null)
			{ controller = targetGameObject.AddComponent<CharacterController>(); }
			else if(controllerType == ControllerType.rvoController && controller2 == null)
			{
				if(AstarPath.HasPro)
				{
					controller2 = targetGameObject.AddComponent<RVOController>();
					controller2.Move(new Vector3(0,0,0));
				}
				else 
				{
					controllerType = ControllerType.characterController;//free version can't use RVOControllers

					if(controller == null)
					{ controller = targetGameObject.AddComponent<CharacterController>(); }
				}
			}			
			else if(controllerType == ControllerType.rigidbody && rigidbody == null)
			{
				rigidbody = targetGameObject.AddComponent<Rigidbody>();
				rigidbody.drag = 0.5f;
				rigidbody.freezeRotation = true;
			}
			else if(controllerType == ControllerType.rigidbodyVelocity && rigidbody == null)
			{
				rigidbody = targetGameObject.AddComponent<Rigidbody>();
				rigidbody.freezeRotation = true;
			}	
			
			if(moveMode != MoveMode.followPath)
			{ CalculatePath(); }
			
			if(moveMode == MoveMode.followPath && !startAtStart.Value)
			{ currentWaypoint = GetClosestIndex(); }
			
			if(moveMode == MoveMode.followPath && connectPath.Value)
			{ ConnectPathX(); }
      	}
		
		public void ConnectPathX()
		{
			abPath = ABPath.Construct(targetGameObject.transform.position , path.vectorPath[currentWaypoint] , FinishConnectionX);
			AstarPath.StartPath (abPath);
		}  
		
		public void FinishConnectionX(Path path)
		{
			path.path.InsertRange(currentWaypoint,abPath.path);// node path, necessary if you want to run modifiers on top of it lateron I think
			path.vectorPath.InsertRange(currentWaypoint,abPath.vectorPath);// concat lists by adding everything at the currentWaypoint index . That way nothing before this needs to be reset
			newFsmPath = new FsmPath();
			newFsmPath.Value = path;
			OutputPath.Value = newFsmPath;			
			return;
		}
		
		public int GetClosestIndex()
		{
			a =  1/0f;
			var x = 1;
			var o = 1f;
			
			for (var i = 0; i < (path.vectorPath).Count; i++)
			{
				o = (path.vectorPath[i] - targetGameObject.transform.position).sqrMagnitude;
				if(o < a) 
				{
					a = o;
					x = i;
				}
			}
			return x;
		}
		
		public void UpdateCurrentWaypoint()
		{
			var x = false;
			var y = false;
			var z = false;
			
			var dir = path.vectorPath[currentWaypoint+1] - path.vectorPath[currentWaypoint];
			if(dir.x<=0) x = true;
			if(dir.y<=0) y = true;
			if(dir.z<=0) z = true;
			
			var actorDir = targetGameObject.transform.position - path.vectorPath[currentWaypoint + 1];
			if((x ? (actorDir.x<=0) : (actorDir.x>0))&&(y ? (actorDir.y<=0) : (actorDir.y>0))&&(z ? (actorDir.z<=0) : (actorDir.z>0)))
			{ currentWaypoint++; }
		}
		
		public void CalculatePath()
		{
			if(moveMode != MoveMode.follow && moveMode != MoveMode.followTo && moveMode != MoveMode.fleeContinuously)
			{ path = null; }
						
			Vector3 targetPos;
			if (target.Value != null)
			{ targetPos = target.Value.transform.position + targetPosition.Value;}
			else
			{ targetPos = targetPosition.Value;}
			
			if(path != null && path.vectorPath.Count > 0 && (moveMode == MoveMode.follow || moveMode == MoveMode.followTo))	
			{abPath = ABPath.Construct (nextPos , targetPos , OnPathComplete);} // create path from next waypoint (to avoid jitter due to sudden direction change on path update) to closest/first node
			else if(moveMode != MoveMode.fleeContinuously && moveMode != MoveMode.flee && moveMode != MoveMode.randomPath )
			{abPath = ABPath.Construct (targetGameObject.transform.position , targetPos , OnPathComplete);} // create path from current position to closest/first node
			
			else if(moveMode == MoveMode.fleeContinuously || moveMode == MoveMode.flee ) 
			{
				if(AstarPath.HasPro)
				{ abPath = FleePath.Construct (targetGameObject.transform.position , targetPos , (int)(length.Value*1000f), OnPathComplete);} // create path from current position to closest/first node
				else
				{ abPath = ABPath.Construct (targetGameObject.transform.position , targetGameObject.transform.position + (targetGameObject.transform.position - targetPos).normalized * length.Value, OnPathComplete);}
			}
			else if (moveMode == MoveMode.randomPath)
			{
				if(AstarPath.HasPro)
				{ abPath = RandomPath.Construct (targetGameObject.transform.position, (int)(length.Value*1000f) , OnPathComplete); } // create path from current position to closest/first node
				else // random direction! This is just a cheap immitation of the real randompath!
				{ abPath = ABPath.Construct (targetGameObject.transform.position , targetGameObject.transform.position + new Vector3(UnityEngine.Random.Range(-1f,1f), 0f, UnityEngine.Random.Range(-1f,1f)).normalized * length.Value, OnPathComplete);}
			}
				
			AstarPath.StartPath(abPath); //make the actual vector3 path which we'll use lateron.
			time = Time.time;
		}
			
		public void OnPathComplete(Path path1) 
		{
			if (targetGameObject == null) 
			{
				Finish(); 
				return; 
			}
		
			newFsmPath = new FsmPath();
			path = path1;
			
			currentWaypoint = 1;
			if(moveMode == MoveMode.followPath)
			{ currentWaypoint = 0; }// used to be getClosest() + 1;
			else if(moveMode == MoveMode.follow || moveMode == MoveMode.followTo || moveMode == MoveMode.fleeContinuously)
			{ currentWaypoint = (path.vectorPath.Count >= 2 && (path.vectorPath[0]-targetGameObject.transform.position).sqrMagnitude <= (path.vectorPath[1]-targetGameObject.transform.position).sqrMagnitude) ? 0 : 1; }

			newFsmPath.Value = path;
			OutputPath.Value = newFsmPath;
			
			if (LogEvents.Value)
			{ Debug.Log(" Time needed to create path : " + (Time.time - time)); }
			
			pathLoading = false;
			if(exactFinish.Value)
			{ path.vectorPath.Add(target.Value == null ? targetPosition.Value : target.Value.transform.position);}
		}
		
		public void ShadowExtendPath()
		{
			if((target.Value != null && (target.Value.transform.position - path.vectorPath[path.vectorPath.Count - 1]).sqrMagnitude >= shadowUpdateDistance.Value * shadowUpdateDistance.Value))
            {
				path.vectorPath.Add(target.Value.transform.position);
				path.path.Add(AstarPath.active.GetNearest(target.Value.transform.position).node);
			}
			else if((target.Value == null && (targetPosition.Value - path.vectorPath[path.vectorPath.Count - 1]).sqrMagnitude >= shadowUpdateDistance.Value * shadowUpdateDistance.Value))
            {
				path.vectorPath.Add(targetPosition.Value);
				path.path.Add(AstarPath.active.GetNearest(targetPosition.Value).node);
			}
		}
		
		public void Auto() 
        {
			var moveOnPathComponent = targetGameObject.GetComponent<FsmMoveOnPath>() ?? targetGameObject.AddComponent<FsmMoveOnPath>();
		    moveOnPathComponent.go = targetGameObject;
			moveOnPathComponent.InputPath = path;
			moveOnPathComponent.speed = speed.Value;
			moveOnPathComponent.finishDistance = finishDistance.Value;
			moveOnPathComponent.nextWaypointDistance = nextWaypointDistance.Value;
			moveOnPathComponent.failureTolerance = failureTolerance.Value;
			moveOnPathComponent.ignoreY = ignoreY.Value;
			moveOnPathComponent.LogEvents = LogEvents.Value;
			moveOnPathComponent.currentWaypoint = currentWaypoint;
			moveOnPathComponent.costDependendSpeed = costDependendSpeed.Value;
		}
		
		public void Move() 
		{
			if(controllerType == ControllerType.rvoController || (controllerType == ControllerType.available && controller2 != null))
			{
				if (controller2 != null) 
				{
					controller2.Move(direction);
					controller2.maxSpeed = (direction).magnitude; 
				}
				else
				{
					if(LogEvents.Value)
					{ Debug.Log("Astar Move To failed. The controller was removed"); }
					
					Fsm.Event(failedEvent); 
					Finish(); 
					return;
				}
			}
			
			else if(controllerType == ControllerType.characterController || (controllerType == ControllerType.available && controller != null))
			{
				if (controller != null) 
				{ controller.SimpleMove(direction); }
				else
				{
					if(LogEvents.Value)
					{ Debug.Log("Astar Move To failed. The controller was removed"); }
					Fsm.Event(failedEvent); Finish(); return;
				}
			}

			else if(controllerType == ControllerType.rigidbody || controllerType == ControllerType.rigidbodyVelocity)
			{
				if (controllerType == ControllerType.rigidbody && rigidbody != null) 
				{ rigidbody.AddForce(direction*Time.deltaTime*100*rigidbody.mass); }
				
				else if (controllerType == ControllerType.rigidbodyVelocity && rigidbody != null) 
				{
					if(ignoreY.Value)
					{ rigidbody.velocity = new Vector3(direction.x,rigidbody.velocity.y,direction.z)*110; }
					else
					{ rigidbody.velocity = direction*Time.deltaTime*110; }
					
					rigidbody.WakeUp();
				}
				else
				{
					if(LogEvents.Value)
					{	Debug.Log("Astar Move To failed. The rigidbody controller was removed");} 
					Fsm.Event(failedEvent); 
					Finish(); 
					return;
				}
			}
			
			else if(controllerType == ControllerType.transform || (controllerType == ControllerType.available && controller2 == null && controller == null && rigidbody == null))
			{ targetGameObject.transform.position += direction * Time.deltaTime; }
		}
		
	 	public override void OnUpdate()
	 	{
			if(updatePath && moveMode == MoveMode.followPath)
			{ path = inputPath.GetPath(); }

			if (path == null || path.vectorPath.Count == 0)
            { return; }
			
			if(!pathLoading && ((moveMode == MoveMode.follow || moveMode == MoveMode.followTo || moveMode == MoveMode.fleeContinuously) && frame >= Math.Max(1,updateInterval.Value)) )
			{
				CalculatePath();
				pathLoading = true;
				frame = 0;
			}
			else 
			{ frame += 1; }
			
			if(moveMode == MoveMode.shadow || moveMode == MoveMode.shadowTo ) 
			{ ShadowExtendPath(); }
			
			if(auto.Value)
			{
				Auto();
				if (endOfPathEvent != null) 
				{ Fsm.Event(endOfPathEvent); }
				Finish();
			}

			if (currentWaypoint >= (path.vectorPath).Count) 
			{ currentWaypoint = path.vectorPath.Count-1; }

	 	    var isCloseEnoughToEndPoint = (finishDistanceMode == FinishDistance.absolute && (
                (target.Value != null && (target.Value.transform.position - targetGameObject.transform.position).sqrMagnitude <= finishDistance.Value*finishDistance.Value) ||
	 	        (ignoreY.Value && target.Value != null && (new Vector3(target.Value.transform.position.x, targetGameObject.transform.position.y, target.Value.transform.position.z)
                - targetGameObject.transform.position).sqrMagnitude <= finishDistance.Value*finishDistance.Value) ||
                (target.Value == null && Vector3.Distance(targetGameObject.transform.position, targetPosition.Value) <= finishDistance.Value) ||
                (ignoreY.Value && target.Value == null && (new Vector3(targetPosition.Value.x, targetGameObject.transform.position.y, targetPosition.Value.z) - targetGameObject.transform.position).sqrMagnitude <= finishDistance.Value * finishDistance.Value)));

	 	    var isCloseEnoughToEndNode = (finishDistanceMode == FinishDistance.absoluteEndnode && ((!ignoreY.Value && Vector3.Distance(targetGameObject.transform.position, path.vectorPath[path.vectorPath.Count - 1]) <= finishDistance.Value) ||
	 	        (ignoreY.Value && Vector3.Distance(new Vector3(targetGameObject.transform.position.x, path.vectorPath[path.vectorPath.Count - 1].y, targetGameObject.transform.position.z), path.vectorPath[path.vectorPath.Count - 1]) <=
	 	         finishDistance.Value)));

            if (isCloseEnoughToEndPoint || isCloseEnoughToEndNode)
			{ 
				//Debug.Log("Finish");
			    if (moveMode == MoveMode.follow || moveMode == MoveMode.shadow || moveMode == MoveMode.fleeContinuously) 
                { return; }
			    
                if (LogEvents.Value)
			    { Debug.Log ("End Of path reached."); }
					
			    if (controller2 != null && controllerType == ControllerType.rvoController) //RVO controller needs to be set to 0/0/0 , else it continues running.
			    { controller2.Move(new Vector3(0,0,0)); }	
					
			    if(rigidbody != null && (controllerType == ControllerType.rigidbody || controllerType == ControllerType.rigidbodyVelocity))
			    { rigidbody.velocity = new Vector3(0,rigidbody.velocity.y,0); }
	
			    if(endOfPathEvent != null)
			    { Fsm.Event(endOfPathEvent);} 
					
			    Finish();
			    return;
			}

	 	    if(finishDistanceMode == FinishDistance.relative )
	 	    {
	 	        var i = currentWaypoint;
	 	        var leng = 0.0f;
	 	        while(i<path.vectorPath.Count-1)
	 	        {
	 	            leng+= Vector3.Distance(path.vectorPath[currentWaypoint], path.vectorPath[currentWaypoint+1]);
	 	            if(leng>finishDistance.Value) // if the distance is still too far to finish, break to save performance
	 	            { break; }
	 	        }

	 	        if (!(leng <= finishDistance.Value)) 
                { return; }

	 	        if (moveMode == MoveMode.follow || moveMode == MoveMode.shadow || moveMode == MoveMode.fleeContinuously)
                { return; }

	 	        if (LogEvents.Value)
	 	        { Debug.Log ("End Of path reached."); }
	 	        if (controller2 != null && controllerType == ControllerType.rvoController) //RVO controller needs to be set to 0/0/0 , else it continues running.
	 	        { controller2.Move(new Vector3(0,0,0));	}
	 	        if(rigidbody != null &&(controllerType == ControllerType.rigidbody || controllerType == ControllerType.rigidbodyVelocity))
	 	        { rigidbody.velocity = new Vector3(0,rigidbody.velocity.y,0); }
							
	 	        Fsm.Event(endOfPathEvent);
	 	        Finish();
	 	        return;
	 	    }
            
	 	    // Check if we are close enough to the next waypoint.
			if(ignoreY.Value) 
			{
				var distanceVector = nextPos - targetGameObject.transform.position ;
				distanceVector.y = 0;
				distance = distanceVector.magnitude;
			}
			else
			{ distance = Vector3.Distance(targetGameObject.transform.position, nextPos); }

			if(distance < nextWaypointDistance.Value && !smoothTurns.Value) 
			{	
				if(finishDistanceMode == FinishDistance.last && currentWaypoint >= (path.vectorPath).Count - 1) 
				{
					if(moveMode != MoveMode.follow && moveMode != MoveMode.shadow && moveMode != MoveMode.fleeContinuously)
					{
						if (LogEvents.Value)
						{ Debug.Log ("End Of path reached."); }
						
						if (controller2 != null && controllerType == ControllerType.rvoController) //RVO controller needs to be set to 0/0/0 , else it continues running.
						{ controller2.Move(new Vector3(0,0,0));	}
						
						if(rigidbody != null &&(controllerType == ControllerType.rigidbody || controllerType == ControllerType.rigidbodyVelocity))
						{ rigidbody.velocity = new Vector3(0,rigidbody.velocity.y,0); }
							
						Fsm.Event(endOfPathEvent);
						Finish();
						return;
					}
				}
				
				currentWaypoint++;
				currentWaypoint = Math.Min(currentWaypoint,path.vectorPath.Count-1);
			}

			nextPos = path.vectorPath[currentWaypoint];
			
			// Direction to the next waypoint.
			if(!smoothTurns.Value)
			{ direction = (nextPos - targetGameObject.transform.position).normalized; }
			else 
			{
				var targetPos = prevTarget;
				if(frame == 1)
				{
					if(firstFrame)
					{	targetPos = targetGameObject.transform.position; } // keep targetPos on update 

					currentWaypoint = 1;
					path.vectorPath[0] = targetPos;
				}
				currentWaypoint = Math.Min(currentWaypoint,path.vectorPath.Count-2);
				var deltaPos = 0.0f;
				
				if( frame == 1) 
				{ deltaPos = turnRadius.Value - (targetPos - targetGameObject.transform.position).magnitude; }
				else 
				{ deltaPos = (targetGameObject.transform.position - prevPosition).magnitude; }
				
				
				if(deltaPos*deltaPos > (path.vectorPath[currentWaypoint+1]-targetPos).sqrMagnitude)
				{
					while (deltaPos*deltaPos > (path.vectorPath[currentWaypoint+1]-targetPos).sqrMagnitude) 
					{
						currentWaypoint++;
						currentWaypoint = Math.Min(currentWaypoint, path.vectorPath.Count - 2);
						targetPos = path.vectorPath[currentWaypoint];
						deltaPos -= (path.vectorPath[currentWaypoint]-targetPos).magnitude;
					}
				}
				
				if ((targetPos-targetGameObject.transform.position).sqrMagnitude < turnRadius.Value*turnRadius.Value)
				{ targetPos += ((path.vectorPath[currentWaypoint+1]-targetPos).normalized) * deltaPos *1.5f ; }

				prevPosition = targetGameObject.transform.position;
				prevTarget = targetPos;
				direction = (targetPos - targetGameObject.transform.position).normalized;
			}

			directionOut.Value = direction;
			
			if (ignoreY.Value)
			{
				direction.y = 0;
				directionOut.Value = new Vector3(directionOut.Value.x, 0, directionOut.Value.z);
				directionOut.Value = directionOut.Value.normalized;
				direction = direction.normalized;
			}
			
			outputSpeed.Value = (float)((1/Math.Exp(costDependendSpeed.Value * path.path[Math.Min(currentWaypoint,path.path.Count-1)].penalty)  ) * speed.Value);
			direction *= outputSpeed.Value; // 1/e^x for exponentially slower speed, but never 0 or negative or more than 1. Math classes were good for something afterall :D
			
			Move();
			
			firstFrame = false;
			
			if (drawGizmos && path != null && path.path != null && path.vectorPath != null) 
			{
				for (var i=0;i<path.vectorPath.Count-1;i++) 
				{ Debug.DrawLine (path.vectorPath[i] ,path.vectorPath[i+1],gizmosColor.Value); }
			}
		}
		
		public override void OnExit() 
		{
			if (controller2 != null)
			{ controller2.maxSpeed = 0; }
		}		
	}
}