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
        
        public static float AngleFromOffset(Vector2 inputVector)
        {
            float angle = Vector2.Angle(Vector2.right, inputVector);

            if (inputVector.y < 0)
            {
                angle = 360 - angle;
            }

            float finalAngle = Mathf.Atan2(inputVector.y, inputVector.x) * Mathf.Rad2Deg;

            if (finalAngle < 0)
            {
                finalAngle += 360;
            }

            return finalAngle;
        }

  
    }
}