using UnityEngine;

namespace Behaviors.AI.States
{
    [CreateAssetMenu(fileName = "IdleBehavior", menuName = "IA/Behavior/Idle")]
    public class IdleBehavior : StateBehavior
    {
        public override void BehaveFixedUpdate(AIController controller)
        {
            base.BehaveFixedUpdate(controller);
        }
    }
}