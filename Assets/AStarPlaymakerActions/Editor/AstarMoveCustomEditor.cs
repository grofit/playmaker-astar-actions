using System;
using HutongGames.PlayMaker.Pathfinding.Enums;
using HutongGames.PlayMakerEditor;
using UnityEngine;

namespace HutongGames.PlayMaker.Pathfinding.Editor
{
    [CustomActionEditor(typeof(MoveTo))]
    public class AstarMoveCustomEditor : CustomActionEditor
    {	
        public override bool OnGUI()
        {	
            var moveToTarget = target as MoveTo;
		
            EditField("targetObjectHelper");
		
            EditField("moveMode");
            if(moveToTarget.moveMode == MoveMode.flee || moveToTarget.moveMode == MoveMode.fleeContinuously || moveToTarget.moveMode == MoveMode.randomPath)
            { EditField("length"); }

            if(moveToTarget.moveMode == MoveMode.follow || moveToTarget.moveMode == MoveMode.followTo || moveToTarget.moveMode == MoveMode.fleeContinuously)
            { EditField("updateInterval"); }

            if(moveToTarget.moveMode == MoveMode.shadow || moveToTarget.moveMode == MoveMode.shadowTo)
            { EditField("shadowUpdateDistance"); }	
		
            EditField("actor");
            if(moveToTarget.moveMode == MoveMode.followPath)
            {
                EditField("inputPath");
                EditField("updatePath");		
            }

            if(moveToTarget.moveMode != MoveMode.followPath && moveToTarget.moveMode != MoveMode.randomPath)
            {
                EditField("target");
                EditField("targetPosition");
            }

            EditField("controllerType");
            EditField("auto");
            if(moveToTarget.moveMode == MoveMode.followPath)
            {
                EditField("startAtStart");
                EditField("connectPath");
            }
		
            EditField("ignoreY");
            EditField("speed");
		
            EditField("smoothTurns");
            if(moveToTarget.smoothTurns.Value)
            { EditField("turnRadius"); }

            EditField("costDependendSpeed");
            
            if(!moveToTarget.smoothTurns.Value)
                EditField("nextWaypointDistance"); 
		
            EditField("finishDistanceMode");
            if(moveToTarget.moveMode != MoveMode.follow && moveToTarget.moveMode != MoveMode.fleeContinuously)
            { EditField ("endOfPathEvent"); }

            EditField("finishDistance");
		
            if(!(moveToTarget.moveMode == MoveMode.flee || moveToTarget.moveMode == MoveMode.fleeContinuously || moveToTarget.moveMode == MoveMode.randomPath))
                EditField("exactFinish");
		
            EditField("failedEvent");
		
            if(moveToTarget.moveMode != MoveMode.follow && moveToTarget.moveMode != MoveMode.fleeContinuously)
            { EditField("failureTolerance"); }

            EditField ("directionOut");
		
            EditField("OutputPath");
            EditField("outputSpeed");
		
            EditField("LogEvents");
            EditField ("drawGizmos");

            if(moveToTarget.drawGizmos)
            { EditField("gizmosColor"); }	
		
            return GUI.changed;
        }	
    }
}