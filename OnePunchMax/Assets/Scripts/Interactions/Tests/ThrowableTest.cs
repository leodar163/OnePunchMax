using UnityEngine;
using UnityEngine.Events;

namespace Interactions.Tests
{
    public class ThrowableTest : MonoBehaviour, IThrowable
    {
        [SerializeField] private Rigidbody2D _rb;
        public Vector3 Position => transform.position;

        private Vector2 _thrownPos;
        private bool _isThrown;
        private float _range;
        
        [SerializeField] public UnityEvent onThrown;
        [SerializeField] public UnityEvent onHitFinishThrow;
        
        public void OnInteract(IInteractor interactor)
        {
            
        }

        public void OnHover()
        {
            
        }

        private void FixedUpdate()
        {
            if (_isThrown && Vector2.Distance(transform.position, _thrownPos) >= _range)
            {
                StopThrowing();
            }
        }

        public void OnThrown(IThrower thrower)
        {
            _rb.AddForce(thrower.Direction * thrower.Force, ForceMode2D.Impulse);
            _rb.angularVelocity = thrower.SpineForce;
            _thrownPos = transform.position;
            _range = thrower.Range;
            _isThrown = true;
            onThrown.Invoke();
        }   

        private void StopThrowing()
        {
            _isThrown = false;
            _rb.velocity = Vector2.zero;
            _rb.angularVelocity = 0;
            onHitFinishThrow.Invoke();
        }
    }
}