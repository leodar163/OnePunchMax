using UnityEngine;

namespace Behaviors.AI.States
{
    
    public abstract class StateBehavior : ScriptableObject
    {
        //[SerializeField] protected StateBehavior[] _childStateBehaviors;
        [SerializeField] protected StateBehavior _nextState;
        [SerializeField] protected bool PushNextStateWithHesitation;
        
        public virtual void EnterState(AIController controller)
        {
            
        }
        
        public virtual void FixedUpdate(AIController controller)
        {
            /*
            foreach (var state in _childStateBehaviors)
            {
                state.FixedUpdate(controller);
            }
            */
        }
        
        public virtual void Update(AIController controller)
        {
            /*
            foreach (var state in _childStateBehaviors)
            {
                state.Update(controller);
            }
            */
        }

        public virtual void LateUpdate(AIController controller)
        {
            /*
            foreach (var state in _childStateBehaviors)
            {
                state.LateUpdate(controller);
            }
            */
        }

        public virtual void CancelState(AIController controller)
        {
            controller.PushState(null);
        }
        
        public virtual void ExitState(AIController controller)
        { 
            if(PushNextStateWithHesitation) 
                controller.PushStateWithHesitation(_nextState);
            else
                controller.PushState(_nextState);
        }
    }
}