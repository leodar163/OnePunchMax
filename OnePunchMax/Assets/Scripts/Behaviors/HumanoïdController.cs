using System;
using Behaviors.Attack;
using Interactions;
using UnityEngine;
using UnityEngine.Events;

namespace Behaviors
{
    public abstract class HumanoïdController : MonoBehaviour, IInteractor
    {
        [Header("Rotation")]
        [SerializeField] protected LookAtBehavior _lookAtBehavior;
        [Header("Interactions")]
        [SerializeField] protected InteractableDetector _interactableDetector;
        [SerializeField] protected ObjectHolder _holder;
        [SerializeField] protected ObjectThrower _thrower;
        [Header("Attack")] 
        [SerializeField] protected AttackBehavior[] _attacks;

        [SerializeField] public UnityEvent OnAttack;

        public Vector2 AimingDirection { get; protected set; }


        protected virtual void FixedUpdate()
        {
            _lookAtBehavior.LookTo((Vector2)transform.position + AimingDirection);
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

        public void Attack(int attackType = 0)
        {
            if (_attacks.Length <= 0 || attackType >= _attacks.Length) return;
          
            _attacks[attackType]?.Attack();
            
            OnAttack.Invoke();
        }
    }
}