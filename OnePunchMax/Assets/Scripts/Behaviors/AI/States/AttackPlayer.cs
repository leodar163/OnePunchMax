namespace Behaviors.AI.States
{
    public class AttackPlayer : StateBehavior
    {
        public override void Update(AIController controller)
        {
            if (controller.IsPlayerInRange())
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