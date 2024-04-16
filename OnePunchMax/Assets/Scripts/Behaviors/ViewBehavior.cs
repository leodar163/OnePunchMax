using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Utils.Debug;
using Utils.Debug.Gizmos;
using Collider2D = UnityEngine.Collider2D;

namespace Behaviors
{
    public class ViewBehavior : MonoBehaviour
    {
        [Header("Vision cone")]
        [SerializeField][Range(0,360)] private float _viewAngle;
        [SerializeField][Range(-180,180)] private float _viewOffset;
        [SerializeField][Min(0)] private float _viewRadius;

        [Header("Detection settings")]
        [Tooltip("Defines the number of frame before one detection occur")]
        [SerializeField] [Min(0)] private int _rateOfDetection = 6;
        private int _frameRecord;
        [SerializeField] private ContactFilter2D _filter;
        [SerializeField][Min(0)] private int _maxNumberOfDetection = 4;

        [Space] [Header("Occlusion")] 
        [SerializeField] private bool _activateOcclusion = true;
        [Tooltip("Defines what will block the view detection")]
        [SerializeField] private ContactFilter2D _occlusionFilter;
        [Header("Debug")]
        [SerializeField] private DebugData _debugData;
        [Header("Events")]
        [SerializeField] public UnityEvent<Collider2D> onColliderEnters;
        [SerializeField] public UnityEvent<Collider2D> onColliderExits;

        private Transform _transform;

        private Transform cachedTransform
        {
            get
            {
                if (_transform == null) _transform = transform;
                return _transform;
            }
        }
            
        private Collider2D[] _colliders;

        private Collider2D[] Colliders
        {
            get
            {
                if (_colliders == null || _colliders.Length != _maxNumberOfDetection)
                    _colliders = new Collider2D[_maxNumberOfDetection];

                return _colliders;
            }
        }

        private readonly RaycastHit2D[] _occlusionHits = new RaycastHit2D[1];
        
        private List<Collider2D> _previousColliders = new();
        
        private void OnDrawGizmosSelected()
        {
            if (!_debugData.activateVisualDebug) return;
            
            Gizmos.color = _debugData.gizmoColor;
            GizmosExtensions.Draw2DCone(cachedTransform.position, _viewRadius, _viewAngle, _viewOffset + cachedTransform.rotation.eulerAngles.z);
        }

        private void FixedUpdate()
        {
            if (_frameRecord >= _rateOfDetection)
            {
                Detect();
                _frameRecord = 0;
            }
            
            _frameRecord++;
        }

        private void Detect()
        {
            List<Collider2D> filteredColliders = new();
            
            if (Physics2D.OverlapCircle(transform.position, _viewRadius, _filter, Colliders) > 0)
            {
                foreach (var col in _colliders)
                {
                    if (col == null) continue;
                    
                    Vector2 position = cachedTransform.position;
                    Vector2 closestPoint = col.ClosestPoint(position);
                    Vector2 relativePos = closestPoint - position;
                    
                    float angle = Mathf.Atan2(relativePos.y, relativePos.x) * Mathf.Rad2Deg;

                    if (Mathf.Abs(Mathf.DeltaAngle(_viewOffset + cachedTransform.rotation.eulerAngles.z, angle)) <=
                        _viewAngle / 2)
                    {
                        if (!_activateOcclusion)
                        {
                            filteredColliders.Add(col);
                            if (_debugData.activateVisualDebug) Debug.DrawLine(position, closestPoint, Color.green, 0.128f);
                            continue;
                        }
                        
                        if (Physics2D.Linecast(position, closestPoint, _occlusionFilter, _occlusionHits) == 0)
                        {
                            filteredColliders.Add(col);
                            if (_debugData.activateVisualDebug) Debug.DrawLine(position, closestPoint, Color.green, 0.128f);
                        }
                        else if (_debugData.activateVisualDebug)
                        {
                            Debug.DrawLine(position, _occlusionHits[0].point, Color.green, 0.128f);
                            Debug.DrawLine(_occlusionHits[0].point, closestPoint, Color.red, 0.128f);
                        }
                    }
                }

                foreach (var col in filteredColliders)
                {
                    if (!_previousColliders.Remove(col))
                    {
                        onColliderEnters.Invoke(col);
                        if (_debugData.activateVisualDebug) print($"{col.name} entered view");
                    }
                }
            }
            
            foreach (var col in _previousColliders)
            {
                onColliderExits.Invoke(col);
                if (_debugData.activateVisualDebug) print($"{col.name} exited view");
            }

            _previousColliders = filteredColliders;
        }
    }
}