﻿using System;
using Inputs;
using UnityEngine;

namespace Characters
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Behaviors")]
        [SerializeField] private MovementBehavior _movementBehavior;
        [SerializeField] private LookAtBehavior _lookAtBehavior;
        [Header("Aiming")]
        [SerializeField] private Transform _aimePoint;
        [SerializeField] private float _aimingRadius;

        private Vector2 _aimingDirection;
        private Vector2 _lastMousePosition;
        
        private Camera _mainCam;
        
        
        private void Awake()
        {
            _mainCam = Camera.main;
        }

        private void FixedUpdate()
        {
            DefineAimingDirection();

            Vector2 moveInput = InputsUtility.MainControls.Movements.Move.ReadValue<Vector2>();
            
            if (moveInput.magnitude > 0.125f) 
                _movementBehavior.MoveToward(moveInput);
            
            _lookAtBehavior.LookTo((Vector2)transform.position + _aimingDirection);
            PlaceAimePoint();
        }

        private void DefineAimingDirection()
        {
            Vector2 direction = InputsUtility.MainControls.Movements.Aime.ReadValue<Vector2>();
            Vector2 currentMousePosition = Input.mousePosition;
            Vector2 mouseWorldPos = _mainCam.ScreenToWorldPoint(currentMousePosition);
            
            if (direction.magnitude > 0.125f)
            {
                _aimingDirection = direction.normalized;
            }
            else if (Vector2.Distance(currentMousePosition, _lastMousePosition) > 0.125f)
            {
                _aimingDirection = mouseWorldPos - (Vector2)transform.position;
                if (Vector2.Distance(mouseWorldPos, transform.position) > 1)
                    _aimingDirection = _aimingDirection.normalized;
            }

            _lastMousePosition = currentMousePosition;
        }
        
        private void PlaceAimePoint()
        {
            Transform aimeTransform = _aimePoint.transform;
            aimeTransform.position = transform.position + (Vector3)_aimingDirection * _aimingRadius;
            aimeTransform.rotation = new Quaternion();
        }
    }   
}