using Detections;

namespace Interactions
{
    public interface IInteractable : IPositionnable
    {
        public void OnInteract(IInteractor interactor);

        public void OnHover();
    }
}
