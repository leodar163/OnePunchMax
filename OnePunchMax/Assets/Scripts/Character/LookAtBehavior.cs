using System;
using UnityEngine;

namespace Character
{
    public class LookAtBehavior : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rb;
        
        [Header("Look at")] 
        [SerializeField][Min(0)] public float maxRotationSpeed;
        [SerializeField][Min(0)] public float acceleration;
        public Vector2 direction { get; private set; }
        private Vector2 currentDirection;

        private bool _hasReceivedInput;
        
        private void FixedUpdate()
        {
            ManagerLookAt();
            
            if (!_hasReceivedInput)
                direction = currentDirection;
            _hasReceivedInput = false;
        }

        private void ManagerLookAt()
        {
            currentDirection = _rb.transform.rotation * Vector3.right;
            
            float currentRotation = Mathf.Atan2(currentDirection.y, currentDirection.x) * Mathf.Rad2Deg;

            float targetRotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            float rotationDelta = Mathf.DeltaAngle(currentRotation, targetRotation);

            float rotationVelocity = rotationDelta * Mathf.Abs(rotationDelta) * acceleration;
            
            rotationVelocity = Mathf.Clamp(rotationVelocity, -maxRotationSpeed, maxRotationSpeed);
            
            _rb.angularVelocity = rotationVelocity;
        }
        
        public void LookTo(Vector2 positionToLookAt)
        {
            direction = (positionToLookAt - (Vector2)_rb.transform.position).normalized;
            _hasReceivedInput = true;
        }
    }
}