﻿using Interactions;
using UnityEngine;

namespace Behaviors.AI.States
{
    [CreateAssetMenu(fileName = "SeekPickableBehavior", menuName = "IA/Behavior/Seek Pickable")]
    public class SeekPickable : StateBehavior
    {
        [SerializeField] private StateBehavior _whenNoPickableInView;
        public override void BehaveUpdate(AIController controller)
        {
            if ( controller.GetNearestInteractableInRange() is IPickable nearestPickable)
            {
                controller.AimAt(nearestPickable.Position);
                controller.InteractWith(nearestPickable);
                ExitState(controller);
            }
            else if (controller.GetNearestInteractableInView() is IPickable nearestPickableInView)
            {
                controller.AimAt(nearestPickableInView.Position);
                controller.MoveTo(nearestPickableInView.Position);
            }
            else
            {
                if (_pushNextStateWithHesitation)
                    controller.PushStateWithHesitation(_whenNoPickableInView);
                else
                    controller.PushState(_whenNoPickableInView);
            }
            base.BehaveUpdate(controller);
        }
    }
}