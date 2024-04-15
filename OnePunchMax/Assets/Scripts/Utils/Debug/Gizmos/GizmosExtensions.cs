using UnityEngine;

namespace Utils.Debug.Gizmos
{
    public static class GizmosExtensions
    {
        public static void Draw2DCone(Vector3 center, float radius, float angle, float offset, int angleDefinition = 24)
        {
            angle *= Mathf.Deg2Rad;
            offset *= Mathf.Deg2Rad;

            angleDefinition = Mathf.Abs(angleDefinition);
            
            Vector3 previousPoint = new Vector3
            {
                x = Mathf.Cos(-angle / 2 + offset),
                y = Mathf.Sin(-angle / 2 + offset)
            } * radius + center;
            
            UnityEngine.Gizmos.DrawLine(center, previousPoint);

            float stepAngle = angle / angleDefinition;
            
            for (int i = 0; i <= angleDefinition; i++)
            {
                Vector3 nextPoint = new Vector3
                {
                    x = Mathf.Cos(stepAngle * i + offset - angle/2),
                    y = Mathf.Sin(stepAngle * i + offset - angle/2)
                } * radius + center;
                
                UnityEngine.Gizmos.DrawLine(previousPoint, nextPoint);
                previousPoint = nextPoint;
            }
            
            UnityEngine.Gizmos.DrawLine(center, previousPoint);
        }
    }
}