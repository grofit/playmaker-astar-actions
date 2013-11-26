using UnityEngine;
using System;
using HutongGames.PlayMaker.Pathfinding;
using Pathfinding;

namespace HutongGames.PlayMaker.Pathfinding
{
    [ActionCategory("A Star")]
    [Tooltip("move an object from its' current position directly to the target position or gameObject")]
    public class CalculateETA : FsmStateAction
    {
        [ObjectType(typeof (FsmPath))] [Tooltip("If the path is unequal null , move along this path. Else use the target or target position")] 
        public FsmObject inputPath;
        
        public FsmFloat speed = 20f;
        
        public FsmFloat costDependendSpeed = 0f;

        public FsmFloat estimatedTime = 0f;

        public override void OnEnter()
        {
            estimatedTime.Value = 0f;
            var path = (inputPath.Value as FsmPath).Value;
            var prevPos = new Vector3(0f, 0f, 0f);

            foreach (Node node in path.path)
            {
                var nodePosition = new Vector3(node.position.x, node.position.y, node.position.z)*Int3.PrecisionFactor;
                if (prevPos != new Vector3(0f, 0f, 0f))
                {
                    var distance = (nodePosition - prevPos).magnitude;
                    var cost = Math.Exp(costDependendSpeed.Value*node.penalty);
                    estimatedTime.Value += (distance*(float)cost)/speed.Value;
                }
                prevPos = nodePosition;
            }
            Finish();
        }
    }
}