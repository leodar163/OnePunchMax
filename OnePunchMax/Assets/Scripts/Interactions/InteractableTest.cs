using UnityEngine;

namespace Interactions
{
    public class InteractableTest : MonoBehaviour, IInteractable
    {
        public Vector3 Position => transform.position;
        
        public void Interact(InteractableDetector interactableDetector)
        {
            
        }
    }
}