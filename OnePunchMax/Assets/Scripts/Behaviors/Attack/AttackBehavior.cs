using Detections;
using UnityEngine;

namespace Behaviors.Attack
{
    public class AttackBehavior : DetectorSubscriberFiltered<AttackReceiver>
    {
        [SerializeField] private float _reachableNbr = 2;
        [SerializeField] private AttackData _attackData;

        protected override void Awake()
        {
            base.Awake();
            _attackData.attacker = this;
        }

        public void Attack()
        {
            for (int i = 0; i < _inRanges.Count && i < _reachableNbr - 1; i++)
            {
                Attack(_inRanges[i]);
            }
        }
        
        private void Attack(AttackReceiver receiver)
        {
            receiver.ReceiveAttack(_attackData);
        }
    }
}