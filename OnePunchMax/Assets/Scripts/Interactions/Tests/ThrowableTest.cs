using UnityEngine;
using UnityEngine.Events;

namespace Interactions.Tests
{
    public class ThrowableTest : MonoBehaviour, IThrowable
    {
        [SerializeField] private Rigidbody2D _rb;
        
        public Vector3 Position => transform.position;

        [SerializeField] public UnityEvent onThrown;
        [SerializeField] public UnityEvent onHitSomething;
        
        public void OnInteract(IInteractor interactor)
        {
            
        }

        public void OnHover()
        {
            
        }

        public void OnThrown(IThrower thrower)
        {
            _rb.AddForce(thrower.Direction * thrower.Force, ForceMode2D.Impulse);
            onThrown.Invoke();
        }

        public void OnHitSomething()
        {
            onHitSomething.Invoke();
        }
    }
}