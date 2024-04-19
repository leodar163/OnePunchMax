using Behaviors.Attack;
using Detections;
using UnityEngine;
using UnityEngine.Events;

namespace Interactions
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile : MonoBehaviour, IPositionnable
    {
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] public AttackData attackData;

        [SerializeField] public UnityEvent<ITarget> onHitTarget;
        public Vector3 Position => transform.position;
        public Vector2 Direction
        {
            get => _rb.velocity.normalized;
            set => _rb.velocity = value.normalized * Force;
        }

        public float Force
        {
            get => _rb.velocity.magnitude;
            set => _rb.velocity = Direction * value;
        }

        private void OnValidate()
        {
            if (_rb == null) TryGetComponent(out _rb);
        }

        private void Awake()
        {
            if (_rb == null) TryGetComponent(out _rb);
        }

        protected virtual void OnHitTarget(ITarget target)
        {
            attackData.source = this;
            target.ReceiveAttack(attackData);
            onHitTarget.Invoke(target);
        }

        protected virtual void OnCollisionEnter2D(Collision2D other)
        {
            _rb.angularVelocity = 0;
            _rb.velocity = Vector2.zero;
            
            if (other.collider.TryGetComponent(out ITarget target))
            {
                OnHitTarget(target);
            }
        }
    }
}