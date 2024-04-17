using System.Collections;
using Behaviors.AI.States;
using Interactions;
using UnityEngine;
using UnityEngine.AI;

namespace Behaviors.AI
{
    public class AIController : HumanoidController
    {
        [SerializeField] public InteractableDetector _InteractableViewer;
        [SerializeField] public NavMeshAgent _navMeshAgent;
        [SerializeField] public StateBehavior CurrentState;

        [SerializeField] private float _hesitationTime = 1;
        private IEnumerator _hesitationRoutine;
            
        protected override void FixedUpdate()
        {
            CurrentState.FixedUpdate(this);
            base.FixedUpdate();
        }

        private void Update()
        {
            CurrentState.Update(this);
        }

        private void LateUpdate()
        {
            CurrentState.LateUpdate(this);
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

        public bool IsPlayerInRange()
        {
            return _attacks[0]?.Nearest != null;
        }

        public void PushState(StateBehavior state)
        {
            if (CurrentState) CurrentState.CancelState(this);
            CurrentState = state;
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
            CurrentState = null;
            
            yield return new WaitForSeconds(_hesitationTime);
            
            _hesitationRoutine = null;
            PushState(state);
        }
    }
}