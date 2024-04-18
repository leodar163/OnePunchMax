using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Utils.Debug;

namespace Detections
{
    public abstract class DetectorSubscriberFiltered<T> : DetectorSubscriber where T : class, IPositionnable
    {
        protected List<T> _inRanges = new();

        private T _nearest;
        public T Nearest => _nearest;
        
        [SerializeField] private Vector2 _distanceOffset;
        [SerializeField] private DebugData _debugData;

        [SerializeField] public UnityEvent<T> onNewNearest; 
        
        protected virtual void OnDrawGizmosSelected()
        {
            if (!_debugData.activateVisualDebug) return;
            Gizmos.color = _debugData.gizmoColor;
            Gizmos.DrawSphere(transform.position + transform.rotation * _distanceOffset , 0.15f);
        }

        protected virtual void LateUpdate()
        {
            _inRanges.RemoveAll(item => item == null || item.Equals(null));
            T oldNearest = Nearest;
            SortInRangesByPosition();
            if ((oldNearest == null && Nearest != null) || (oldNearest != null && oldNearest != Nearest) || (oldNearest != null && Nearest == null))
            {
                onNewNearest.Invoke(Nearest);
            }
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
            if (_inRanges.Count < 2)
            {
                _nearest = _inRanges.Count > 0 ? _inRanges[0] : null;
                return;
            }

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

            _nearest = _inRanges.Count > 0 ? _inRanges[0] : null;
        }
    }
}