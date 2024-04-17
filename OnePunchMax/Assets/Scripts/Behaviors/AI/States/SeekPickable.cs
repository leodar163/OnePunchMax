using Interactions;
using UnityEngine;
using Utils;

namespace Behaviors.AI.States
{
    [CreateAssetMenu(fileName = "SeekPickableBehavior", menuName = "IA/Behavior/Seek Pickable")]
    public class SeekPickable : StateBehavior
    {
        public override void Update(AIController controller)
        {
            if ( controller.GetNearestInteractableInRange() is IPickable nearestPickable)
            {
                controller.InteractWith(nearestPickable);
                ExitState(controller);
            }
            else if (controller.GetNearestInteractableInView() is IPickable nearestPickableInView)
            {
                controller.MoveTo(nearestPickableInView.RigidBody.position);
            }
            else
            {
                ExitState(controller);
            }
            base.Update(controller);
        }
    }
}