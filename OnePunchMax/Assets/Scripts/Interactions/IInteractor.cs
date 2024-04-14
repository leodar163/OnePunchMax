using UnityEngine;

namespace Interactions
{
    public interface IInteractor
    {
        public Transform transform { get; }


        public void InteractWith(IInteractable interactable);
    }
}