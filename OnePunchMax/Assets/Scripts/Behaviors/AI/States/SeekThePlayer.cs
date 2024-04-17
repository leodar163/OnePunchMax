using UnityEngine;
using Utils;

namespace Behaviors.AI.States
{
    public class SeekThePlayer : StateBehavior
    {
        [SerializeField] [Min(0.001f)] private float _stopDistance;
        [SerializeField] private SeekInteractables _seekInteractables;
            
        public override void Update(AIController controller)
        {
            
            Vector3 playerPosition = Singleton<PlayerController>.Instance.transform.position;
            if (Vector2.Distance(playerPosition, controller.transform.position) <= _stopDistance)
            {
                ExitState(controller);
            }
            else
            {
                controller.MoveTo(playerPosition);
            }
            base.Update(controller);
            
            if (_seekInteractables != null && controller.GetNearestInteractableInView() != null)
            {
                controller.PushState(_seekInteractables);
            }
        }
    }
}