using System.Collections.Generic;
using UnityEngine;
using Utils.Debug;

namespace Detections
{
    public abstract class DetectorSubscriberFiltered<T> : DetectorSubscriber where T : class, IPositionnable
    {
        protected List<T> _inRanges = new();

        public T Nearest => _inRanges.Count > 0 ? _inRanges[0] : null;
        
        [SerializeField] private Vector2 _distanceOffset;
        [SerializeField] private DebugData _debugData;
        
        protected virtual void OnDrawGizmosSelected()
        {
            if (!_debugData.activateVisualDebug) return;
            Gizmos.color = _debugData.gizmoColor;
            Gizmos.DrawSphere(transform.position + transform.rotation * _distanceOffset , 0.15f);
        }

        protected virtual void LateUpdate()
        {
            _inRanges.RemoveAll(item => item == null || item.Equals(null));
            SortInRangesByPosition();
        }

        protected override void OnColliderEnters(Collider2D col)
        {
            if (col.TryGetComponent(out T filtered))
                AddFiltered(filtered);
        }

        protected override void OnColliderExits(Collider2D col)
        {
            if (col.TryGetComponent(out T filtered))
                RemoveFiltered(filtered);
        }

        private void AddFiltered(T filtered)
        {
            if (filtered == null || _inRanges.Contains(filtered)) return;
            _inRanges.Add(filtered);
        }

        private void RemoveFiltered(T filtered)
        {
            if (filtered == null || !_inRanges.Remove(filtered)) return;
        }

        private void SortInRangesByPosition()
        {
            if (_inRanges.Count < 2) return;

            List<T> sorteds = new List<T>();
            
            while (_inRanges.Count > 0)
            {
                T nearest = null;
                
                float nearestDistance = float.PositiveInfinity;
            
                foreach (var inRange in _inRanges.ToArray())
                {
                    float distance = Vector3.Distance(transform.position, inRange.Position);

                    if (distance < nearestDistance)
                    {
                        nearestDistance = distance;
                        nearest = inRange;
                    }
                }

                _inRanges.Remove(nearest);
                sorteds.Add(nearest);
            }

            _inRanges = sorteds;
        }
    }
}