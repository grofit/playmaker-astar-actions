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
            if(moveToTarget.moveMode == MoveMode.Flee || moveToTarget.moveMode == MoveMode.FleeContinuously || moveToTarget.moveMode == MoveMode.RandomPath)
            { EditField("length"); }

            if(moveToTarget.moveMode == MoveMode.Follow || moveToTarget.moveMode == MoveMode.FollowTo || moveToTarget.moveMode == MoveMode.FleeContinuously)
            { EditField("updateInterval"); }

            if(moveToTarget.moveMode == MoveMode.Shadow || moveToTarget.moveMode == MoveMode.ShadowTo)
            { EditField("shadowUpdateDistance"); }	
		
            EditField("actor");
            if(moveToTarget.moveMode == MoveMode.FollowPath)
            {
                EditField("inputPath");
                EditField("updatePath");		
            }

            if(moveToTarget.moveMode != MoveMode.FollowPath && moveToTarget.moveMode != MoveMode.RandomPath)
            {
                EditField("target");
                EditField("targetPosition");
            }

            EditField("controllerType");
            EditField("auto");
            if(moveToTarget.moveMode == MoveMode.FollowPath)
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
            if(moveToTarget.moveMode != MoveMode.Follow && moveToTarget.moveMode != MoveMode.FleeContinuously)
            { EditField ("endOfPathEvent"); }

            EditField("finishDistance");
		
            if(!(moveToTarget.moveMode == MoveMode.Flee || moveToTarget.moveMode == MoveMode.FleeContinuously || moveToTarget.moveMode == MoveMode.RandomPath))
                EditField("exactFinish");
		
            EditField("failedEvent");
		
            if(moveToTarget.moveMode != MoveMode.Follow && moveToTarget.moveMode != MoveMode.FleeContinuously)
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