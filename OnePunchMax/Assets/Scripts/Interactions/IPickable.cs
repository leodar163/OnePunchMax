using UnityEngine;

namespace Interactions
{
    public interface IPickable : IInteractable
    {
        public Rigidbody2D RigidBody { get; }
        
        public bool IsPickable { get; set; }

        public void OnPicked(IHolder holder);

        public void OnDropped(IHolder holder);
    }
}