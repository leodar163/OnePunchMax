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

        private void Update()
        {
            ManagerLookAt();
        }

        private void ManagerLookAt()
        {
            float directionAngle = Mathf.Atan2(_controller.AimingDirection.y, _controller.AimingDirection.x) * Mathf.Rad2Deg;
            float direction = Mathf.DeltaAngle(0, directionAngle) switch
            {
                < 45 and >= -45 => 1,
                >= 45 and < 135 => 2,
                >= 135 or <-135 => 3,
                < -45 and >= - 135 => 0,
                
                _ => throw new ArgumentOutOfRangeException()
            };
            
            //_animator.SetFloat(DirectionAnimArg, direction);
            _animator.SetFloat(SpeedAnimArg, _controller.GetMovement().magnitude > 0.01f ? 1 : 0);
        }
    }
}