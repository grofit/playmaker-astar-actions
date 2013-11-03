using System;
using HutongGames.PlayMaker.Pathfinding.Enums;
using HutongGames.PlayMakerEditor;
using UnityEngine;

namespace HutongGames.PlayMaker.Pathfinding.Editor
{
    [CustomActionEditor(typeof(setGraphInfo))]
    public class SetGraphCustomEditor : CustomActionEditor
    {
        public override bool OnGUI()
        {
            var targetGraphInfo = target as setGraphInfo;
            if(targetGraphInfo == null)
            { throw new NullReferenceException("Target Graph Info is null"); }
		
            EditField("graphType");
            EditField("graph");
            EditField("drawGizmos");
            EditField("infoScreenOpen");
            EditField("open");
            EditField("initialPenalty");
            EditField("name");
            EditField("nodes");
            EditField("everyFrame");	
		
            if(targetGraphInfo.graphType == GraphType.pointGraph)
            {
                EditField("autoLinkNodes");
                EditField("limits");
                EditField("mask");
                EditField("maxDistance");
                EditField("raycast");
                EditField("recursive");
                EditField("searchTag");
                EditField("thickRaycast");
                EditField("thickRaycastRadius");
            }
            if(targetGraphInfo.graphType == GraphType.gridGraph)
            {
                EditField("size");
                EditField("scans");
                EditField("getNearestForceOverlap");

            }					
            return GUI.changed;
        }	
    }
}