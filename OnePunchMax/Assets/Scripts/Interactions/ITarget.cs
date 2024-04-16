using Behaviors.Attack;
using Detections;

namespace Interactions
{
    public interface ITarget : IPositionnable
    {
        public void ReceiveAttack(AttackData data);
    }
}