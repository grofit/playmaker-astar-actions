using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using HutongGames.PlayMakerEditor;
using HutongGames.PlayMaker.Pathfinding;
using UnityEngine;

[CustomActionEditor(typeof(setGraphInfo))]
public class SetGraphCustomEditor : CustomActionEditor
{
    public override bool OnGUI()
    {
	  	var aTarget = target as setGraphInfo;
		
		EditField("graphType");
		EditField("graph");
		EditField("drawGizmos");
		EditField("infoScreenOpen");
		EditField("open");
		EditField("initialPenalty");
		EditField("name");
		EditField("nodes");
		EditField("everyFrame");	
		
		if(aTarget.graphType == GraphType.pointGraph){
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
		if(aTarget.graphType == GraphType.gridGraph){
			EditField("size");
			EditField("scans");
			EditField("getNearestForceOverlap");

		}					
		return GUI.changed;
    }	
}