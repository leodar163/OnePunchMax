using Detections;

namespace Interactions
{
    public class InteractableDetector : DetectorSubscriberFiltered<IInteractable>
    {
        protected override void LateUpdate()
        {
            base.LateUpdate();
            Nearest?.OnHover();
        }
    }
}