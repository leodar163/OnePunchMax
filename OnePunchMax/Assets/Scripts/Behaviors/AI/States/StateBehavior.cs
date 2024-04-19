using UnityEngine;

namespace Behaviors.AI.States
{
    
    public abstract class StateBehavior : ScriptableObject
    {
        //[SerializeField] protected StateBehavior[] _childStateBehaviors;
        [SerializeField] protected StateBehavior _nextState;
        [SerializeField] protected bool _pushNextStateWithHesitation;

        public bool PushNextStateWithHesitation => _pushNextStateWithHesitation;
        
        public virtual void EnterState(AIController controller)
        {
            
        }
        
        public virtual void BehaveFixedUpdate(AIController controller)
        {
            /*
            foreach (var state in _childStateBehaviors)
            {
                state.FixedUpdate(controller);
            }
            */
        }
        
        public virtual void BehaveUpdate(AIController controller)
        {
            /*
            foreach (var state in _childStateBehaviors)
            {
                state.Update(controller);
            }
            */
        }

        public virtual void BehaveLateUpdate(AIController controller)
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