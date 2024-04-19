using System;
using Behaviors.Attack;
using Interactions;
using UnityEngine;
using UnityEngine.Events;

namespace Behaviors
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class HumanoidController : MonoBehaviour, IInteractor, ITarget
    {
        [SerializeField] protected Rigidbody2D _rb;
        [Header("Interactions")]
        [SerializeField] protected InteractableDetector _interactableDetector;
        [SerializeField] protected ObjectHolder _holder;
        [SerializeField] protected ObjectThrower _thrower;
        [Header("Attack")] 
        [SerializeField] protected AttackBehavior[] _attacks;

        [SerializeField] public UnityEvent<int> OnAttack;
        [SerializeField] public UnityEvent<AttackData> OnReceiveAttack;
        [SerializeField] public UnityEvent OnDie;
        [SerializeField] public UnityEvent OnResurect;
        [SerializeField] public UnityEvent<AttackData> onGetHurt;

        public bool IsDead { get; protected set; }
        public bool IsCharging { get; protected set; }
        public bool FullyCharged { get; protected set; }
        
        public Vector2 AimingDirection { get; protected set; }
        
        public Vector3 Position => transform.position;

        protected virtual void OnValidate()
        {
            if (_rb == null && TryGetComponent(out _rb))
            {
                
            }
        }

        protected virtual void Update()
        {
            _thrower.Direction = AimingDirection;
            OrientateViews();
        }

        protected virtual void OrientateViews()
        {
            Quaternion newRotation =
                Quaternion.Euler(0, 0, Mathf.Atan2(AimingDirection.y, AimingDirection.x) * Mathf.Rad2Deg);

            _interactableDetector.transform.rotation = newRotation;
            _holder.transform.rotation = newRotation;
            _thrower.transform.rotation = newRotation;
            foreach (var attack in _attacks)
            {
                attack.transform.rotation = newRotation;
            }
        }
        
        protected virtual void FixedUpdate()
        {
            
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
                _thrower.Direction = AimingDirection;
                _thrower.Throw(throwable);
            }
        }
        
        public void Attack(int attackType = 0)
        {
            if (_attacks.Length <= 0 || attackType >= _attacks.Length) return;
          
            if (_attacks[attackType] != null && _attacks[attackType].CanAttack)
            {
                _attacks[attackType]?.Attack();

                OnAttack.Invoke(attackType);
                FullyCharged = false;
            }
        }
        
        public virtual void ReceiveAttack(AttackData data)
        {
            if (data.source != null)
            {
                Vector2 attackDirection = (transform.position - data.source.Position).normalized;
                _rb.AddForce(attackDirection * (data.knockBackForce * _rb.mass), ForceMode2D.Impulse);
            }
            OnReceiveAttack.Invoke(data);
        }
        
        public virtual void Die()
        { 
            enabled = false;
            IsDead = true;
            OnDie.Invoke();
        }

        public virtual void Resurect()
        {
            enabled = true;
            IsDead = false;
            OnResurect.Invoke();
        }

        public virtual Vector2 GetMovement()
        {
            return Vector2.zero;
        }
    }
}