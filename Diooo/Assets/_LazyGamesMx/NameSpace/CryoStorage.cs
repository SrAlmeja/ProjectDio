using UnityEngine;

namespace CryoStorage
{
    
    public static class CryoMath
    {
        public static Vector3 PointOnRadius(Vector3 center, float radius, float angle)
        {
            float xOffset = radius * Mathf.Cos(angle);
            float zOffset = radius * Mathf.Sin(angle);
            Vector3 result = new Vector3(center.x + xOffset, center.y, center.z + zOffset);
            return result;
            
        }

        public static Quaternion AimAtDirection(Vector3 center, Vector3 position)
        {
            Vector3 aimDir = position - center;
            Quaternion result = Quaternion.LookRotation(aimDir);
            return result;
        }
  
    }
}