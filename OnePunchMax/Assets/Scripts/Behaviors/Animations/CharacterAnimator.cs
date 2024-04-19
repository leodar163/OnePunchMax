using System;
using UnityEngine;

namespace Behaviors.Animations
{
    public class CharacterAnimator : MonoBehaviour
    {
        [SerializeField] private HumanoidController _controller;
        [SerializeField] private Animator _animator;

        private bool _hasReceivedInput;
        private static readonly int DirectionAnimArg = Animator.StringToHash("Direction");
        private static readonly int SpeedAnimArg = Animator.StringToHash("Speed");
        private static readonly int DeadAnimArg = Animator.StringToHash("IsDead");
        private static readonly int ChargedAnimArg = Animator.StringToHash("FullyCharged");
        private static readonly int ChargingAnimArg = Animator.StringToHash("IsCharging");
        private static readonly int AttackAnimArg = Animator.StringToHash("LightAttack");
        private static readonly int LargeAttackAnimArg = Animator.StringToHash("ChargedAttack");

        [SerializeField] private bool _lookByAim;

        private void Awake()
        {
            _controller.OnAttack.AddListener(SetAttackAnimationTrigger);
        }

        private void OnDisable()
        {
            _controller.OnAttack.RemoveListener(SetAttackAnimationTrigger);
        }

        private void Update()
        {
            ManagerLookAt();
        }

        private void ManagerLookAt()
        {
            float directionAngle = 0;
            if (_lookByAim)
            {
                directionAngle = Mathf.Atan2(_controller.AimingDirection.y, _controller.AimingDirection.x) * Mathf.Rad2Deg;
            }
            else
            {
                Vector2 movementDirection = _controller.GetMovement();
                directionAngle = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg;
            }
            
            float direction = Mathf.DeltaAngle(0, directionAngle) switch
            {
                < 45 and >= -45 => 1,
                >= 45 and < 135 => 2,
                >= 135 or <-135 => 3,
                < -45 and >= - 135 => 0,
                
                _ => throw new ArgumentOutOfRangeException()
            };
            
            _animator.SetFloat(DirectionAnimArg, direction);
            _animator.SetFloat(SpeedAnimArg, _controller.GetMovement().magnitude > 0.01f ? 1 : 0);
            _animator.SetBool(DeadAnimArg, _controller.IsDead);
            _animator.SetBool(ChargingAnimArg, _controller.IsCharging);
            _animator.SetBool(ChargedAnimArg, _controller.FullyCharged);
        }

        public void SwitchLookMode()
        {
            _lookByAim = !_lookByAim;
        }

        private void SetAttackAnimationTrigger(int attackType)
        {
            switch (attackType)
            {
                case 0:
                    _animator.SetTrigger(AttackAnimArg);
                    break;
                case >0:
                    _animator.SetTrigger(LargeAttackAnimArg);
                    break;
            }
        }
    }
}