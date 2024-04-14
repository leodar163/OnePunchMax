using System;
using Inputs;
using UnityEngine;

namespace Characters
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private MovementBehavior _movementBehavior;
        [SerializeField] private LookAtBehavior _lookAtBehavior;

        private Camera _mainCam;
        
        private void Awake()
        {
            _mainCam = Camera.main;
            
        }

        private void FixedUpdate()
        {
            _movementBehavior.MoveToward(InputsUtility.MainControls.Movements.Move.ReadValue<Vector2>());
            _lookAtBehavior.LookTo(_mainCam.ScreenToWorldPoint(Input.mousePosition));
        }
    }
}