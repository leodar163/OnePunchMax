using System;
using System.Collections.Generic;
using UnityEngine;

namespace Detections
{
    public abstract class DetectorSubscriber : MonoBehaviour
    {
        [SerializeField] protected List<ColliderDetector> _detectors = new ();

        protected virtual void Awake()
        {
            foreach (var detector in _detectors)
            {
                detector.onColliderEnters.AddListener(OnColliderEnters);
                detector.onColliderExits.AddListener(OnColliderExits);
            }
        }

        public void SubToDetector(ColliderDetector detector)
        {
            if (detector == null) return;
            if (!_detectors.Contains(detector))
            {
                _detectors.Add(detector);
                detector.onColliderEnters.AddListener(OnColliderEnters);
                detector.onColliderExits.AddListener(OnColliderExits);
            }
        }

        public void UnsubFromDetector(ColliderDetector detector)
        {
            if (detector == null) return;
            if (_detectors.Remove(detector))
            {
                detector.onColliderEnters.RemoveListener(OnColliderEnters);
                detector.onColliderExits.RemoveListener(OnColliderExits);
            }
        }

        public void UnsubAllDetectors()
        {
            foreach (var detector in _detectors.ToArray())
            {
                UnsubFromDetector(detector);
            }
        }

        protected virtual void OnColliderEnters(Collider2D col)
        {
            
        }

        protected virtual void OnColliderExits(Collider2D col)
        {
            
        }
    }
}