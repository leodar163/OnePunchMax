using Behaviors.Attack;
using Environment;
using Inputs;
using UnityEngine;

namespace Behaviors
{
    public class PlayerController : HumanoidController
    {
        [Header("Movement")]
        [SerializeField] private MovementBehavior _movementBehavior;
        [Header("Aiming")]
        [SerializeField] private Transform _aimePoint;
        [SerializeField] private float _aimingRadius;
        [Header("Attack")]
        [SerializeField] private float _timeToChargeAttack;
        private float _timeCharged;

        private Vector2 _lastMousePosition;
        
        private Camera _mainCam;
        
        
        private void Awake()
        {
            _mainCam = Camera.main;
            EnvironmentManager.Player = this;
        }

        protected override void Update()
        {
            if (InputsUtility.MainControls.Actions.Fire.IsPressed() && _holder.HolderSelf.HoldObject == null)
            {
                _timeCharged += Time.deltaTime;
            }
            
            if (InputsUtility.MainControls.Actions.Interact.WasReleasedThisFrame())
            {
               InteractWith(_interactableDetector.Nearest);
            }
            if (InputsUtility.MainControls.Actions.Fire.WasReleasedThisFrame())
            {
                if (_holder.HolderSelf.HoldObject != null) ActivateHoldObject();
                else
                {
                    if (_timeCharged > _timeToChargeAttack)
                    {
                        _timeCharged = 0;
                        _attacks[1]?.Attack();
                    }
                    else
                    {
                        _attacks[0]?.Attack();
                    }
                }
            }
            base.Update();
        }

        protected override void FixedUpdate()
        {
            DefineAimingDirection();

            Vector2 moveInput = InputsUtility.MainControls.Movements.Move.ReadValue<Vector2>();
            
            if (moveInput.magnitude > 0.125f) 
                _movementBehavior.MoveToward(moveInput);
            PlaceAimePoint();
            
            base.FixedUpdate();
        }

        private void DefineAimingDirection()
        {
            Vector2 direction = InputsUtility.MainControls.Movements.Aime.ReadValue<Vector2>();
            Vector2 currentMousePosition = Input.mousePosition;
            Vector2 mouseWorldPos = _mainCam.ScreenToWorldPoint(currentMousePosition);
            
            if (direction.magnitude > 0.125f)
            {
                AimingDirection = direction.normalized;
            }
            else if (Vector2.Distance(currentMousePosition, _lastMousePosition) > 0.125f)
            {
                AimingDirection = mouseWorldPos - (Vector2)transform.position;
                AimingDirection = AimingDirection.normalized;
            }

            _lastMousePosition = currentMousePosition;
        }
        
        private void PlaceAimePoint()
        {
            Transform aimeTransform = _aimePoint.transform;
            aimeTransform.position = transform.position + (Vector3)AimingDirection * _aimingRadius;
            aimeTransform.rotation = new Quaternion();
        }

        public override void ReceiveAttack(AttackData data)
        {
            Destroy(gameObject);
            base.ReceiveAttack(data);
        }
    }   
}