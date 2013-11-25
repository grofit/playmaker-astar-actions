using HutongGames.PlayMaker.Pathfinding.Enums;
using HutongGames.PlayMakerEditor;
using UnityEngine;

namespace HutongGames.PlayMaker.Pathfinding.Editor
{
    [CustomActionEditor(typeof(GetGraphInfo))]
    public class GetGraphCustomEditor : CustomActionEditor
    {
        public override bool OnGUI()
        {
            var aTarget = target as GetGraphInfo;
		
            EditField("graphType");
            EditField("graph");
            EditField("guid");
            EditField("drawGizmos");
            EditField("infoScreenOpen");
            EditField("open");
            EditField("initialPenalty");
            EditField("name");
            EditField("nodes");
            EditField("everyFrame");	
		
            if(aTarget.graphType == GraphType.PointGraph){
                EditField("root");
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
            if(aTarget.graphType == GraphType.GridGraph){
                EditField("size");
                EditField("scans");
                EditField("getNearestForceOverlap");

            }					
            return GUI.changed;
        }	
    }
}