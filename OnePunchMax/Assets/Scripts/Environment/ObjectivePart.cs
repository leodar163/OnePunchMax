using Behaviors.Attack;
using DG.Tweening;
using Interactions;
using System;
using UnityEngine;

namespace Environment
{
    public class ObjectivePart : MonoBehaviour, ITarget
    {
        [SerializeField] private GameObject _triggerFeedback;
        [SerializeField] private SpriteRenderer _sprite;

        [SerializeField] private Color _blinkColor = Color.red;
        [SerializeField] private float _blinkDelay = 1.0f;
        [SerializeField] private AnimationCurve _blinkCurve;

        public Vector3 Position => transform.position;

        public bool IsTriggered
        {
            get => _isTriggered;
            private set
            {
                if (_isTriggered == value) return;
                _isTriggered = value;
                SetView(value);
                Triggered?.Invoke(value);
            }
        }
        private bool _isTriggered;

        public event Action<bool> Triggered;

        public void ReceiveAttack(AttackData data)
        {
            IsTriggered = true;
        }

        private void SetView(bool triggered)
        {
            _triggerFeedback.SetActive(triggered);
            _sprite.DOColor(_blinkColor, _blinkDelay)
                .SetLoops(-1)
                .SetEase(_blinkCurve)
                .Play();
        }
    }
}