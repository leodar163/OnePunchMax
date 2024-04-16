using System;
using System.Collections.Generic;
using UnityEngine;
using Utils.Debug;
using Utils.Debug.Gizmos;

namespace Detections
{
    [Serializable]
    public class DetectionCone : DetectionGeometry
    {
        [SerializeField][Range(0,360)] private float _viewAngle = 90;
        [SerializeField][Range(-180,180)] private float _viewOffset;
        [SerializeField][Min(0)] private float _viewRadius = 3;

        public float ViewAngle
        {
            get => _viewAngle;
            set => _viewAngle = value;
        }

        public float ViewOffset
        {
            get => _viewOffset;
            set => _viewOffset = value;
        }

        public float ViewRadius
        {
            get => _viewRadius;
            set => _viewRadius = value;
        }

        public override void DrawGizmo(Vector3 center, Quaternion rotation)
        {
            GizmosExtensions.Draw2DCone(center, _viewRadius, _viewAngle, _viewOffset + rotation.eulerAngles.z);
        }

        public override List<Collider2D> DetectColliders(Vector3 center, Quaternion rotation, ContactFilter2D filter, Collider2D[] unFilteredColliders,
            bool activateOcclusion = false, ContactFilter2D occlusionFilter = default, DebugData debugData = default)
        {
            List<Collider2D> filteredColliders = new();

            if (Physics2D.OverlapCircle(center, _viewRadius, filter, unFilteredColliders) > 0)
            {
                foreach (var col in unFilteredColliders)
                {
                    if (col == null) continue;

                    Vector2 position = center;
                    Vector2 closestPoint = col.ClosestPoint(position);
                    Vector2 relativePos = closestPoint - position;

                    float angle = Mathf.Atan2(relativePos.y, relativePos.x) * Mathf.Rad2Deg;

                    if (Mathf.Abs(Mathf.DeltaAngle(_viewOffset + rotation.eulerAngles.z, angle)) <=
                        _viewAngle / 2)
                    {
                        if (!activateOcclusion)
                        {
                            filteredColliders.Add(col);
                            if (debugData.activateDebug) Debug.DrawLine(position, closestPoint, Color.green, 0.128f);
                            continue;
                        }

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
            }
            return filteredColliders;
        }
    }
}