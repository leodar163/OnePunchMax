using Behaviors.Attack;
using Detections;
using UnityEngine;

namespace Interactions
{
    public interface ITarget : IPositionnable
    {
        public void ReceiveAttack(AttackData data);
    }
}