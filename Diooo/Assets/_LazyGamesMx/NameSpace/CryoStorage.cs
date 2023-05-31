using UnityEngine;

namespace CryoStorage
{
    public static class CryoMath
    {
        //this hard coded value aligns the angle to the forward vector
        private static float angleOffset = 1.5555555f;
        public static Vector3 PointOnRadius(Vector3 center, float radius, float angle)
        {
            float rad = angle * Mathf.Deg2Rad;
            float xOffset = radius * Mathf.Cos(-rad + angleOffset);
            float zOffset = radius * Mathf.Sin(-rad + angleOffset);
            Vector3 result = new Vector3(center.x + xOffset, center.y, center.z + zOffset);
            return result;
        }
        
        public static Vector3 PointOnRadiusRelative(Transform center, float radius, float angle)
        {
            float rad = (angle + center.eulerAngles.y) * Mathf.Deg2Rad;
            float xOffset = radius * Mathf.Cos(-rad + angleOffset);
            float zOffset = radius * Mathf.Sin(-rad + angleOffset);
            Vector3 result = center.position + new Vector3(xOffset, 0f, zOffset);
            return result;
        }

        public static Quaternion AimAtDirection(Vector3 center, Vector3 position)
        {
            Vector3 aimDir = position - center;
            Quaternion result = Quaternion.LookRotation(aimDir);
            return result;
        }

        public static float InverseMap(float maxValue, float currentValue, float minValue)
        {
            var result = (maxValue - currentValue) / (maxValue - minValue) * (minValue - 1f) + 1f;;
            return result;
        }
        
        public static float InverseMapSkewed(float maxValue, float minValue, float currentValue, float skewFactor)
        {
            float inputRange = maxValue - minValue;
            float resultRange = 1.2f - 0.9f;
            float skewedValue = (currentValue - 1f) * (currentValue > 1f ? skewFactor : -skewFactor) + currentValue;
            float result = (1.2f - skewedValue) / resultRange * inputRange + maxValue;
            return result;
        }
        
        public static float AngleFromOffset(Vector2 vectorInput)
        {
            float angle = Mathf.Atan2(vectorInput.x, vectorInput.y) * Mathf.Rad2Deg;
            if (angle < 0f)
            {
                angle += 360f;
            }
            return angle;
        }
    }
}