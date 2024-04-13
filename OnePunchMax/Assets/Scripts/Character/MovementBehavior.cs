﻿using UnityEngine;

namespace Character
{
    public class MovementBehavior : MonoBehaviour
    {
        [SerializeField] protected Rigidbody2D _rb;

        [Header("Movement")] 
        [SerializeField][Min(0)] public float maxSpeed;
        [SerializeField][Min(0)] public float acceleration;
        [SerializeField][Min(0)] public float deceleration;
        public Vector2 direction { get; private set; }

        protected virtual void FixedUpdate()
        {
            ManageMovements();
        }

        protected virtual void LateUpdate()
        {
            direction = Vector2.zero;
        }
        
        public void MoveToward(Vector2 headedDirection)
        {
            direction = headedDirection.normalized;
        }

        private void ManageMovements()
        {
            Vector2 currentDirection = new Vector2
            {
                x = Mathf.Clamp(_rb.velocity.x,-1, 1),
                y = Mathf.Clamp(_rb.velocity.y,-1, 1)
            };
            
            Vector2 currentAcceleration = direction * (acceleration * Time.fixedDeltaTime);
            Vector2 currentDeceleration = new Vector2
            {
                x = currentDirection.x * direction.x <= 0 ? -currentDirection.x : 0,
                y = currentDirection.y * direction.y <= 0 ? -currentDirection.y : 0
            } * (deceleration * Time.fixedDeltaTime);
            
            _rb.velocity += currentAcceleration + currentDeceleration;
            
            ClampSpeed();
        }

        private void ClampSpeed()
        {
            if (_rb.velocity.magnitude > maxSpeed) _rb.velocity = _rb.velocity.normalized * maxSpeed;
        }
    }
}