

using UnityEngine;

namespace Interactions
{
    public interface IInteractable
    {
        public Vector3 Position { get; }
        public void OnInteract(IInteractor interactor);

        public void OnHover();
    }
}
