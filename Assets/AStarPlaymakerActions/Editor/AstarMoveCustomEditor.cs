using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using HutongGames.PlayMakerEditor;
using HutongGames.PlayMaker.Pathfinding;
using UnityEngine;

[CustomActionEditor(typeof(MoveTo))]
public class AstarMoveCustomEditor : CustomActionEditor
{	
    public override bool OnGUI()
    {	
	  	var aTarget = target as MoveTo;
		
		EditField("targetObjectHelper");
		
		EditField("moveMode");
		if(aTarget.moveMode == MoveMode.flee || aTarget.moveMode == MoveMode.fleeContinuously || aTarget.moveMode == MoveMode.randomPath)
			EditField("length");
		if(aTarget.moveMode == MoveMode.follow || aTarget.moveMode == MoveMode.followTo || aTarget.moveMode == MoveMode.fleeContinuously)
			EditField("updateInterval");	
		if(aTarget.moveMode == MoveMode.shadow || aTarget.moveMode == MoveMode.shadowTo)
			EditField("shadowUpdateDistance");	
		
		EditField("actor");
		if(aTarget.moveMode == MoveMode.followPath){
			EditField("inputPath");
			EditField("updatePath");
				
		}
		if(aTarget.moveMode != MoveMode.followPath && aTarget.moveMode != MoveMode.randomPath)
		{
			EditField("target");
			EditField("targetPosition");
		}
		EditField("controllerType");
		EditField("auto");
		if(aTarget.moveMode == MoveMode.followPath){
			EditField("startAtStart");
			EditField("connectPath");
		}
		EditField("ignoreY");
		EditField("speed");
		
		EditField("smoothTurns");
		if(aTarget.smoothTurns.Value)
			EditField("turnRadius");
		EditField("costDependendSpeed");
		if(!aTarget.smoothTurns.Value)
			EditField("nextWaypointDistance");
		
		EditField("finishDistanceMode");
		if(aTarget.moveMode != MoveMode.follow && aTarget.moveMode != MoveMode.fleeContinuously)
			EditField ("endOfPathEvent");
		EditField("finishDistance");
		
		if(!(aTarget.moveMode == MoveMode.flee || aTarget.moveMode == MoveMode.fleeContinuously || aTarget.moveMode == MoveMode.randomPath))
		EditField("exactFinish");
		
		EditField("failedEvent");
		
		if(aTarget.moveMode != MoveMode.follow && aTarget.moveMode != MoveMode.fleeContinuously){
			EditField("failureTolerance");
		}

		EditField ("directionOut");
		
		
		EditField("OutputPath");
		EditField("outputSpeed");
		
		
		EditField("LogEvents");
		EditField ("drawGizmos");

		if(aTarget.drawGizmos)
			EditField("gizmosColor");	
		
		return GUI.changed;
    }	
}