using System;
using System.Collections.Generic;
using UnityEngine;
using Utils.Debug;

namespace Detections
{
    [Serializable]
    public class DetectionCircle : DetectionGeometry
    {
        [SerializeField]private float _offset;
        [SerializeField][Min(0)] private float _radius;
        
        public float Offset
        {
            get => _offset;
            set => _offset = value;
        }

        public float Radius
        {
            get => _radius;
            set => _radius = value;
        }

        private Vector2 GetCirclePosition(Vector3 center, Quaternion rotation)
        {
            return center + rotation * Vector2.right * _offset;
        }
        
        public override void DrawGizmo(Vector3 center, Quaternion rotation)
        {
            Gizmos.DrawWireSphere(GetCirclePosition(center, rotation), _radius);
        }

        public override List<Collider2D> DetectColliders(Vector3 center, Quaternion rotation, ContactFilter2D filter, Collider2D[] unFilteredColliders,
            bool activateOcclusion = false, ContactFilter2D occlusionFilter = default, DebugData debugData = default)
        {
            List<Collider2D> filteredColliders = new();

            if (Physics2D.OverlapCircle(GetCirclePosition(center, rotation), _radius, filter, unFilteredColliders) > 0)
            {
                foreach (var col in unFilteredColliders)
                {
                    if (col == null) continue;

                    Vector2 position = center;
                    Vector2 closestPoint = col.ClosestPoint(position);

                    if (Physics2D.Linecast(position, closestPoint, occlusionFilter, _occlusionHits) == 0)
                    {
                        filteredColliders.Add(col);
                        if (debugData.activateDebug) Debug.DrawLine(position, closestPoint, Color.green, 0.128f);
                    }
                    else if (debugData.activateDebug)
                    {
                        Debug.DrawLine(position, _occlusionHits[0].point, Color.green, 0.128f);
                        Debug.DrawLine(_occlusionHits[0].point, closestPoint, Color.red, 0.128f);
                    }
                }
            }
            return filteredColliders;
        }
    }
}