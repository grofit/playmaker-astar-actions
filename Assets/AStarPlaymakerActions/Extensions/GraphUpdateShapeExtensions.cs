using Pathfinding;
using UnityEngine;

namespace HutongGames.PlayMaker.Extensions
{
    public static class GraphUpdateShapeExtensions
    {
        public static void RecalculateBox(this GraphUpdateShape graphUpdateShape, GameObject gameObject, Bounds bounds)
        {
            graphUpdateShape.points = new Vector3[8];
            graphUpdateShape.points[0] = gameObject.transform.TransformPoint(new Vector3(bounds.min.x, bounds.min.y, bounds.max.z));
            graphUpdateShape.points[1] = gameObject.transform.TransformPoint(new Vector3(bounds.max.x, bounds.min.y, bounds.max.z));
            graphUpdateShape.points[2] = gameObject.transform.TransformPoint(new Vector3(bounds.max.x, bounds.min.y, bounds.min.z));
            graphUpdateShape.points[3] = gameObject.transform.TransformPoint(new Vector3(bounds.min.x, bounds.min.y, bounds.min.z));
            graphUpdateShape.points[7] = gameObject.transform.TransformPoint(new Vector3(bounds.min.x, bounds.max.y, bounds.min.z));
            graphUpdateShape.points[4] = gameObject.transform.TransformPoint(new Vector3(bounds.min.x, bounds.max.y, bounds.max.z));
            graphUpdateShape.points[5] = gameObject.transform.TransformPoint(new Vector3(bounds.max.x, bounds.max.y, bounds.max.z));
            graphUpdateShape.points[6] = gameObject.transform.TransformPoint(new Vector3(bounds.max.x, bounds.max.y, bounds.min.z));
        }
    }
}