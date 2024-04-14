

using UnityEngine;

namespace Interactions
{
    public interface IInteractable
    {
        public Vector3 Position { get; }
        public void Interact(InteractableDetector interactableDetector); 
    }
}
