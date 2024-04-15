using System;
using System.Collections.Generic;
using UnityEngine;

namespace Interactions
{
    public class InteractableDetector : MonoBehaviour
    { 
        [Header("Detection Settings")]
        [SerializeField] private LayerMask _detectionMask;
        [SerializeField] [Min(0)] private int _maxInteractableDetected = 6; 
        [Tooltip("Detect interactable every <Detection Rate> frame")]
        [SerializeField][Min(0)] private int _detectionRate = 10;
        private int step;
        
        [Header("Overlap Properties")] 
        [SerializeField] private float _offset;
        [SerializeField] private float _radius;

        private Collider2D[] _colliders;
        private readonly List<IInteractable> _inRangeInteractables = new();
        private List<IInteractable> InRangeInteractables => new (_inRangeInteractables);

        private IInteractable _nearestInteractable;

        public IInteractable NearestInteractable => _nearestInteractable;
        
        private Transform _transform;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color  = Color.green;
            Gizmos.DrawWireSphere(transform.position + transform.rotation * Vector2.right * _offset,
                _radius);
        }

        private void Awake()
        {
            _transform = transform;
            _colliders = new Collider2D[_maxInteractableDetected];
        }

        private void FixedUpdate()
        {
            if (step >= _detectionRate)
            {
                DetectInteractableInRangeAndFinNearest();
                step = 0;
            }
            
            step++;
        }


        private void DetectInteractableInRangeAndFinNearest()
        {
            Vector2 overlapCirclePosition = _transform.position + _transform.rotation * Vector2.right * _offset;
            
            Physics2D.OverlapCircle(overlapCirclePosition, _radius,
                new ContactFilter2D { layerMask = _detectionMask }, _colliders);

            _inRangeInteractables.Clear();
            
            _nearestInteractable = null;
            
            float nearestDistance = float.PositiveInfinity;
            
            foreach (var col in _colliders)
            {
                if (col != null && col.TryGetComponent(out IInteractable interactable))
                {
                    _inRangeInteractables.Add(interactable);
                    
                    float distance = Vector3.Distance(overlapCirclePosition, interactable.Position);

                    if (distance < nearestDistance)
                    {
                        nearestDistance = distance;
                        _nearestInteractable = interactable;
                    }
                }
            }
        }
    }
}