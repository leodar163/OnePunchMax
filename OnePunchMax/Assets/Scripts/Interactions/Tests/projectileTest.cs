using UnityEngine;

namespace Interactions.Tests
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class projectileTest : MonoBehaviour, IProjectile
    {
        [SerializeField] private Rigidbody2D _rb;

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

        public void OnHitTarget(Target target)
        {
            target.Die();
            _rb.angularVelocity = 0;
            _rb.velocity = Vector2.zero;
        }
    }
}