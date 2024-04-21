using UnityEngine;
using UnityEngine.Events;

namespace Interactions.Tests
{
    public class PickableTest : MonoBehaviour, IPickable
    {
        [SerializeField] private Rigidbody2D _rb;
        public Vector3 Position => transform.position;
        [SerializeField] public UnityEvent onPicked; 
        [SerializeField] public UnityEvent onDropped;

        public Rigidbody2D RigidBody => _rb;
        public bool IsPickable { get; set; } = true;
        
        public void OnInteract(IInteractor interactor)
        {
            
        }

        public void OnHover()
        {
            
        }

        public void FreezePosition(bool freeze)
        {
            bool freezeRotation = _rb.freezeRotation;
            _rb.constraints = freeze ? RigidbodyConstraints2D.FreezeAll : RigidbodyConstraints2D.None;
            _rb.freezeRotation = freezeRotation;
        }
        
        public void OnPicked(IHolder holder)
        {
            IsPickable = false;
            onPicked.Invoke();
        }

        public void OnDropped(IHolder holder)
        {
            IsPickable = true;
            onDropped.Invoke();
        }
    }
}