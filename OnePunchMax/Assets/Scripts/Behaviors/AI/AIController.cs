﻿using System.Collections;
using Behaviors.AI.States;
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
        [SerializeField] public NavMeshAgent _navMeshAgent;
        [SerializeField] public StateBehavior currentState;
        [SerializeField] private SeekPickable _seekPickable;
        [SerializeField] private float _hesitationTime = 1;
        private IEnumerator _hesitationRoutine;
            
        public bool isInAlertMode { get; private set; }

        [Space] 
        [SerializeField] public UnityEvent onAlterModeGoesOn; 
        
        
        protected override void FixedUpdate()
        {
            if (isInAlertMode)
                AimAtThePlayer();
            else
                AimAtTheMovement();
            
            currentState.FixedUpdate(this);
            base.FixedUpdate();
        }

        private void Update()
        {
            currentState.Update(this);
        }

        private void LateUpdate()
        {
            currentState.LateUpdate(this);
            _navMeshAgent.destination = transform.position;
        }

        public void MoveTo(Vector3 position)
        {
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
            _thrower.Throw(GetHoldThrowable());
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
            if (currentState) currentState.CancelState(this);
            currentState = state;
            state.EnterState(this);
            
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
            
        }

        private void AimAtTheMovement()
        {
            
        }

        public void GoInAlertMode()
        {
            if (!isInAlertMode)
            {
                isInAlertMode = true;
                //_interactableDetector.on new nearest => pushnextstate(seekpickable)
            }
        }
    }
}