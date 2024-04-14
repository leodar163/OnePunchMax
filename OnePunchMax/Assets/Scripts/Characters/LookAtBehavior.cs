using System;
using UnityEngine;

namespace Characters
{
    public class LookAtBehavior : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rb;
        
        [Header("Look at")] 
        [SerializeField][Min(0)] public float maxRotationSpeed;
        [SerializeField][Min(0)] public float acceleration;
        public Vector2 Direction { get; private set; }
        private Vector2 _flatDirection;

        private bool _hasReceivedInput;
        
        private void FixedUpdate()
        {
            ManagerLookAt();
            
            if (!_hasReceivedInput)
                Direction = _flatDirection;
            _hasReceivedInput = false;
        }

        private void ManagerLookAt()
        {
            _flatDirection = _rb.transform.rotation * Vector3.right;
            
            float currentRotation = Mathf.Atan2(_flatDirection.y, _flatDirection.x) * Mathf.Rad2Deg;

            float targetRotation = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;

            float rotationDelta = Mathf.DeltaAngle(currentRotation, targetRotation);

            float rotationVelocity = rotationDelta * Mathf.Abs(rotationDelta) * acceleration;
            
            rotationVelocity = Mathf.Clamp(rotationVelocity, -maxRotationSpeed, maxRotationSpeed);
            
            _rb.angularVelocity = rotationVelocity;
        }
        
        public void LookTo(Vector2 positionToLookAt)
        {
            Direction = (positionToLookAt - (Vector2)_rb.transform.position).normalized;
            _hasReceivedInput = true;
        }
    }
}