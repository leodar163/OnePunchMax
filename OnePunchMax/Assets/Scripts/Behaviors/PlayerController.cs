using Inputs;
using Interactions;
using UnityEngine;

namespace Behaviors
{
    public class PlayerController : MonoBehaviour, IInteractor
    {
        [Header("Behaviors")]
        [SerializeField] private MovementBehavior _movementBehavior;
        [SerializeField] private LookAtBehavior _lookAtBehavior;
        [Header("Aiming")]
        [SerializeField] private Transform _aimePoint;
        [SerializeField] private float _aimingRadius;
        [Header("Interactions")]
        [SerializeField] private InteractableDetector _interactableDetector;
        [SerializeField] private ObjectHolder _holder;
        [SerializeField] private ObjectThrower _thrower;

        private Vector2 _aimingDirection;
        private Vector2 _lastMousePosition;
        
        private Camera _mainCam;
        
        
        private void Awake()
        {
            _mainCam = Camera.main;
        }

        private void Update()
        {
            if (InputsUtility.MainControls.Actions.Interact.WasReleasedThisFrame())
            {
                print(_interactableDetector.NearestInteractable);
               InteractWith(_interactableDetector.NearestInteractable);
            }
            if (InputsUtility.MainControls.Actions.Fire.WasReleasedThisFrame())
            {
                ActivateHoldObject();
            }

            _thrower.Direction = _aimingDirection;
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

        public void InteractWith(IInteractable interactable)
        {
            switch (interactable)
            {
                case IPickable { IsPickable: true } pickable:
                    _holder.HolderSelf.Switch(pickable);
                    break;
                case IThrowable throwable:
                    _thrower.Throw(throwable);
                    break;
                default:
                    interactable?.OnInteract(this);
                    break;
            }
        }

        public void ActivateHoldObject()
        {
            IPickable pickable = _holder.HolderSelf.HoldObject;
            
            if (pickable == null) return;
            
            _holder.HolderSelf.Drop();

            if (pickable.RigidBody.TryGetComponent(out IThrowable throwable))
            {
                _thrower.Throw(throwable);
            }
        }
    }   
}