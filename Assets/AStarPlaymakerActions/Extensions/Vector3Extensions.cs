using UnityEngine;

namespace HutongGames.PlayMaker.Behaviours
{
    public static class Vector3Extensions
    {
        public static bool IsInside(this Vector3 point, Collider collider) // this is actually faster then the Bounnds.Contains. On any smaller scale this can speed it up from 3 to 60 frames :D
        {
            // Use collider bounds to get the center of the collider. May be inaccurate
            // for some colliders (i.e. MeshCollider with a 'plane' mesh)
            var center = collider.bounds.center;

            // Cast a ray from point to center
            var direction = center - point;
            var ray = new Ray(point, direction);

            RaycastHit hitInfo;
            var hit = collider.Raycast(ray, out hitInfo, direction.magnitude);

            // If we hit the collider, point is outside. So we return !hit
            return !hit;
        }
    }
}