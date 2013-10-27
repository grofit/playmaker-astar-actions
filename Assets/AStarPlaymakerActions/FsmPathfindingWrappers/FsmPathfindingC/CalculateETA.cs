using UnityEngine;
using System;
using System.Collections;
using HutongGames.PlayMaker;
using FsmPathfinding;
using Pathfinding;


[ActionCategory("A Star")]
[Tooltip("move an object from its' current position directly to the target position or gameObject")]

public class CalculateETA : HutongGames.PlayMaker.FsmStateAction{


		[ObjectType(typeof(FsmPath))]
		[Tooltip("If the path is unequal null , move along this path. Else use the target or target position")]
		public FsmObject inputPath;
		
		public FsmFloat speed = 20f;
		
		public FsmFloat costDependendSpeed = 0f;
		
		public FsmFloat estimatedTime = 0f;
		
		public override void OnEnter() {
			estimatedTime.Value = 0f;
			Path path = (inputPath.Value as FsmPath).Value;
			Vector3 prevPos = new Vector3(0f,0f,0f);
			foreach (Node a in path.path){
				Vector3 aPos = new Vector3(a.position.x,a.position.y,a.position.z) * Int3.PrecisionFactor;
				if (prevPos != new Vector3(0f,0f,0f)){
					estimatedTime.Value += ((aPos - prevPos).magnitude * (float)(Math.Exp(costDependendSpeed.Value * a.penalty )) )/speed.Value; 
				}
				prevPos = aPos;
			}
			
			Finish();
		}
}
