using System.Collections;
using Detections;
using Interactions;
using UnityEngine;

namespace Behaviors.Attack
{
    public class AttackBehavior : DetectorSubscriberFiltered<ITarget>
    {
        [SerializeField] private float _reachableNbr = 2;
        [SerializeField] private AttackData _attackData;

        [SerializeField] private float _attackDelay = 1f;
        [SerializeField] private float _attackCoolDown = 1f;
        
        private bool _canAttack = true;
        
        public bool CanAttack
        {
            get => _canAttack;
        }

        private IEnumerator _coolDownRoutine;
        
        public void Attack()
        {
            if (!_canAttack) return;
            
            for (int i = 0; i < _inRanges.Count && i < _reachableNbr - 1; i++)
            {
               StartCoroutine(Attack(_inRanges[i]));
            }

            if (_coolDownRoutine != null)
            {
                StopCoroutine(_coolDownRoutine);
            }

            _coolDownRoutine = CoolDown();
            StartCoroutine(_coolDownRoutine);
        }
        
        private IEnumerator Attack(ITarget receiver)
        {
            yield return new WaitForSeconds(_attackDelay);
            if (receiver != null && !receiver.Equals(null)) receiver.ReceiveAttack(_attackData);
        }

        private IEnumerator CoolDown()
        {
            _canAttack = false;
            yield return new WaitForSeconds(_attackCoolDown);
            _canAttack = true;
            _coolDownRoutine = null;
        }
        
    }
}