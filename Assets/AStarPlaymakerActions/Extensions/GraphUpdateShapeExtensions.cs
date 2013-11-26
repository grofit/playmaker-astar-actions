using Pathfinding;
using UnityEngine;

namespace HutongGames.PlayMaker.Extensions
{
    public static class GraphUpdateShapeExtensions
    {
        public static void AssignTransformedPoints(this GraphUpdateShape graphUpdateShape, GameObject gameObject, Bounds bounds)
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

        public static void RecalculatePoints(this GraphUpdateShape graphUpdateShape, GameObject gameObject, Bounds bounds)
        {
            if (gameObject.collider is BoxCollider)
            {
                Debug.Log("It's a box collider");
                var castCollider = (gameObject.collider as BoxCollider);
                bounds.center = castCollider.center;
                bounds.size = castCollider.size;
                graphUpdateShape.AssignTransformedPoints(gameObject, bounds);
            }
            else if (gameObject.collider is MeshCollider)
            {
                Debug.Log("It's a mesh collider!");
                var castCollider = (gameObject.collider as MeshCollider);
                graphUpdateShape.points = castCollider.sharedMesh.vertices;

                for (var i = 0; i < graphUpdateShape.points.Length; i++)
                { graphUpdateShape.points[i] = gameObject.transform.TransformPoint(castCollider.sharedMesh.vertices[i]); }
            }
            else
            {
                Debug.Log("This type of collider is not specifically supported. Using bounds instead...");
                graphUpdateShape.AssignTransformedPoints(gameObject, bounds);
            }
        }
    }
}