using System.Collections;
using Behaviors.AI.States;
using Behaviors.Attack;
using Detections;
using Interactions;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Utils;

namespace Behaviors.AI
{
    public class AIController : HumanoidController
    {
        [SerializeField] public InteractableDetector _InteractableViewer;
        [SerializeField] public ColliderDetector _playerViewer;
        [SerializeField] public NavMeshAgent _navMeshAgent;
        [SerializeField] public StateBehavior currentState;
        [SerializeField] private SeekThePlayer _seekThePlayer;
        [SerializeField] private SeekPickable _seekPickable;
        [SerializeField] private float _hesitationTime = 1;
        private IEnumerator _hesitationRoutine;
            
        public bool isInAlertMode { get; private set; }

        [Space] 
        [SerializeField] public UnityEvent onAlertModeGoesOn; 
        private bool _hasReceivedDestinationInput;
        
        protected override void FixedUpdate()
        {
            if (isInAlertMode)
                AimAtThePlayer();
            else
                AimAtTheMovement();
            
            if (currentState != null) currentState.BehaveFixedUpdate(this);
            base.FixedUpdate();
        }

        protected override void Update()
        {
            if (currentState != null) currentState.BehaveUpdate(this);
            base.Update();
        }

        private void LateUpdate()
        {
            if (currentState != null) currentState.BehaveLateUpdate(this);
            if (!_hasReceivedDestinationInput)
            {
                _navMeshAgent.destination = transform.position;
            }
            _hasReceivedDestinationInput = false;
        }

        protected override void OrientateViews()
        {
            Quaternion newRotation =
                Quaternion.Euler(0, 0, Mathf.Atan2(AimingDirection.y, AimingDirection.x) * Mathf.Rad2Deg);
            
            base.OrientateViews();
            _interactableDetector.transform.rotation = newRotation;
            _playerViewer.transform.rotation = newRotation;
        }

        public void MoveTo(Vector3 position)
        {
            _hasReceivedDestinationInput = true;
            _navMeshAgent.destination = position;
        }

        public IInteractable GetNearestInteractableInRange()
        {
            return _interactableDetector.Nearest;
        }
        
        public IInteractable GetNearestInteractableInView()
        {
            return _InteractableViewer.Nearest;
        }
        
        public IThrowable GetHoldThrowable()
        {
            IPickable holdObject = _holder.HolderSelf.HoldObject;
           
            if (holdObject != null && holdObject.RigidBody.TryGetComponent(out IThrowable throwable))
            {
                return throwable;
            }

            return null;
        }

        public void ThrowHoldObject(Vector2 direction)
        {
            AimingDirection = direction;
            ActivateHoldObject();
        }
        
        public bool IsPlayerInAttackRange()
        {
            return _attacks[0]?.Nearest != null;
        }
        
        public bool IsPlayerInThrowRange()
        {
            return Vector2.Distance(transform.position, Singleton<PlayerController>.Instance.transform.position) <=
                   _thrower.Range;
        }

        public void PushState(StateBehavior state)
        {
            currentState = state; 
            //print(state);
            if(currentState != null) currentState.EnterState(this);
            
            if (_hesitationRoutine != null) StopCoroutine(_hesitationRoutine);
        }
        
        public void PushStateWithHesitation(StateBehavior state)
        {
            if (_hesitationRoutine != null)
            {
                StopCoroutine(_hesitationRoutine);
            }

            _hesitationRoutine = Hesitate(state);
            StartCoroutine(_hesitationRoutine);
        }

        private IEnumerator Hesitate(StateBehavior state)
        {
            currentState = null;
            
            yield return new WaitForSeconds(_hesitationTime);
            
            _hesitationRoutine = null;
            PushState(state);
        }

        private void AimAtThePlayer()
        {
            AimingDirection = (Singleton<PlayerController>.Instance.transform.position - transform.position).normalized;
        }

        private void AimAtTheMovement()
        {
            AimingDirection = (_navMeshAgent.destination - transform.position).normalized;
        }

        public void GoInAlertMode()
        {
            if (!isInAlertMode)
            {
                isInAlertMode = true;
                PushState(_seekThePlayer);
                _InteractableViewer.onNewNearest.AddListener(PushSeekPickable);
            }
        }

        private void PushSeekPickable(IInteractable detectedInteractable)
        {
            if (detectedInteractable is IPickable)
            {
                PushStateWithHesitation(_seekPickable);
            }
        }
        
        public override void ReceiveAttack(AttackData data)
        {
            Die();
            base.ReceiveAttack(data);
        }

        public override Vector2 GetMovement()
        {
            return _navMeshAgent.velocity;
        }
    }
}