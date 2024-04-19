using UnityEngine;
using Utils;

namespace Behaviors.AI.States
{
    [CreateAssetMenu(fileName = "AttackPlayerBehavior", menuName = "IA/Behavior/Attack Player")]
    public class AttackPlayer : StateBehavior
    {
        public override void BehaveUpdate(AIController controller)
        {
            controller.AimAt(Singleton<PlayerController>.Instance.transform.position);
            if (controller.GetHoldThrowable() != null)
            {
                if (controller.IsPlayerInThrowRange())
                {
                    Vector2 direction = (Singleton<PlayerController>.Instance.transform.position -
                                         controller.transform.position).normalized;
                    controller.ThrowHoldObject(direction.normalized);
                    ExitState(controller);
                }
                else
                {
                    controller.MoveTo(Singleton<PlayerController>.Instance.transform.position);
                }
                
            }
            else if (controller.IsPlayerInAttackRange())
            {
                controller.Attack();
                ExitState(controller);
            }
            else
            {
                ExitState(controller);
            }
            base.BehaveUpdate(controller);
        }
    }
}