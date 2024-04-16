using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Utils.Debug;
using Utils.Debug.Gizmos;
using Collider2D = UnityEngine.Collider2D;

namespace Detections
{
    public class ColliderDetector : MonoBehaviour
    {
        [SerializeField] [HideInInspector] private DetectionGeometryType _geometryType;
        [SerializeField][HideInInspector] private DetectionCone _cone;
        [SerializeField][HideInInspector] private DetectionCircle _circle;
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
        
        private List<Collider2D> _previousColliders = new();

        public DetectionGeometryType GeometryType
        {
            get => _geometryType;
            set => _geometryType = value;
        }

        public DetectionGeometry Geometry
        {
            get
            {
                return _geometryType switch
                {
                    DetectionGeometryType.circle => _circle,
                    DetectionGeometryType.cone => _cone,
                    DetectionGeometryType.none => null,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (!_debugData.activateDebug) return;
            
            Gizmos.color = _debugData.gizmoColor;
            Geometry?.DrawGizmo(cachedTransform.position, cachedTransform.rotation);
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
            if (Geometry == null) return;

            List<Collider2D> filteredColliders = Geometry.DetectColliders(cachedTransform.position,
                cachedTransform.rotation, _filter, Colliders, _activateOcclusion, _occlusionFilter, _debugData);
            
            foreach (var col in filteredColliders)
            {
                if (!_previousColliders.Remove(col))
                {
                    onColliderEnters.Invoke(col);
                    if (_debugData.activateDebug) print($"{col.name} entered view");
                }
            }
            
            foreach (var col in _previousColliders)
            {
                onColliderExits.Invoke(col);
                if (_debugData.activateDebug) print($"{col.name} exited view");
            }

            _previousColliders = filteredColliders;
        }
    }
}