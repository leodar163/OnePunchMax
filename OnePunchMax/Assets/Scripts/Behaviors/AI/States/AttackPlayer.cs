using UnityEngine;
using Utils;

namespace Behaviors.AI.States
{
    [CreateAssetMenu(fileName = "AttackPlayerBehavior", menuName = "IA/Behavior/Attack Player")]
    public class AttackPlayer : StateBehavior
    {
        public override void BehaveUpdate(AIController controller)
        {

            if (controller.GetHoldThrowable() != null && controller.IsPlayerInThrowRange())
            {
                Vector2 direction = Singleton<PlayerController>.Instance.transform.position -
                                    controller.transform.position;
                controller.ThrowHoldObject(direction.normalized);
                ExitState(controller);
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