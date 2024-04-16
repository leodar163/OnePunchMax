using Detections;
using Interactions;
using UnityEngine;

namespace Behaviors.Attack
{
    public class AttackBehavior : DetectorSubscriberFiltered<ITarget>
    {
        [SerializeField] private float _reachableNbr = 2;
        [SerializeField] private AttackData _attackData;

        public void Attack()
        {
            for (int i = 0; i < _inRanges.Count && i < _reachableNbr - 1; i++)
            {
                Attack(_inRanges[i]);
            }
        }
        
        private void Attack(ITarget receiver)
        {
            if (receiver == null || receiver.Equals(null)) return;
            print($"attack on {receiver}");
            receiver.ReceiveAttack(_attackData);
        }
    }
}