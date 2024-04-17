using UnityEngine;
using Utils;

namespace Behaviors.AI.States
{
    [CreateAssetMenu(fileName = "AttackPlayerBehavior", menuName = "IA/Behavior/Attack Player")]
    public class AttackPlayer : StateBehavior
    {
        public override void Update(AIController controller)
        {

            if (controller.GetHoldThrowable() != null && controller.IsPlayerInThrowRange())
            {
                Vector2 direction = Singleton<PlayerController>.Instance.transform.position -
                                    controller.transform.position;
                controller.ThrowHoldObject(direction.normalized);
            }
            else if (controller.IsPlayerInAttackRange())
            {
                controller.Attack();
            }
            else
            {
                ExitState(controller);
            }
            base.Update(controller);
        }
    }
}