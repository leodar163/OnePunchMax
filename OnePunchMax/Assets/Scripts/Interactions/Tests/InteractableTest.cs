using UnityEngine;

namespace Interactions.Tests
{
    public class InteractableTest : MonoBehaviour, IInteractable
    {
        public Vector3 Position => transform.position;
        
        public void OnInteract(IInteractor interactor)
        {
            
        }

        public void OnHover()
        {
            
        }
    }
}