using System.Collections.Generic;
using Detections;
using UnityEngine;
using Utils.Debug;

namespace Interactions
{
    public class InteractableDetector : DetectorSubscriber
    {
        [SerializeField] private Vector2 _distanceOffset;
        private readonly List<IInteractable> _inRangeInteractables = new();
        private List<IInteractable> InRangeInteractables => new (_inRangeInteractables);

        private IInteractable _nearestInteractable;

        public IInteractable NearestInteractable => _nearestInteractable;
        
        private Transform _cachedTransform;

        [SerializeField] private DebugData _debugData;
        
        private void OnDrawGizmosSelected()
        {
            if (!_debugData.activateDebug) return;
            Gizmos.color = _debugData.gizmoColor;
            Gizmos.DrawSphere(transform.position + transform.rotation * _distanceOffset , 0.15f);
        }

        protected override void Awake()
        {
            base.Awake();
            _cachedTransform = transform;
        }

        private void LateUpdate()
        {
            FindNearestInteractable();
        }

        protected override void OnColliderEnters(Collider2D col)
        {
            if (col.TryGetComponent(out IInteractable interactable))
                AddInteractable(interactable);
        }

        protected override void OnColliderExits(Collider2D col)
        {
            if (col.TryGetComponent(out IInteractable interactable))
                RemoveInteractable(interactable);
        }

        private void AddInteractable(IInteractable interactable)
        {
            if (interactable == null || _inRangeInteractables.Contains(interactable)) return;
            _inRangeInteractables.Add(interactable);
        }

        private void RemoveInteractable(IInteractable interactable)
        {
            if (interactable == null || !_inRangeInteractables.Remove(interactable)) return;
        }
        
        private void FindNearestInteractable()
        {
            _nearestInteractable = null;
         
            if (_inRangeInteractables.Count == 0) return;
            if (_inRangeInteractables.Count == 1)
            {
                _nearestInteractable = _inRangeInteractables[0];
                return;
            }
            
            float nearestDistance = float.PositiveInfinity;
            
            foreach (var interactable in _inRangeInteractables)
            {
                _inRangeInteractables.Add(interactable);
                    
                float distance = Vector3.Distance(_cachedTransform.position, interactable.Position);

                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    _nearestInteractable = interactable;
                }
            }

            _nearestInteractable?.OnHover();
        }
    }
}