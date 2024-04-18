using UnityEngine;
using Utils;

namespace Behaviors.AI.States
{
    [CreateAssetMenu(fileName = "SeekPlayerBehavior", menuName = "IA/Behavior/Seek Player")]
    public class SeekThePlayer : StateBehavior
    {
        [SerializeField] [Min(0.001f)] private float _stopDistance;
            
        public override void BehaveUpdate(AIController controller)
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
            base.BehaveUpdate(controller);
        }
    }
}