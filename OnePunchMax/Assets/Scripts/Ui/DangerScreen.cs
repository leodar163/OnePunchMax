using Behaviors;
using DG.Tweening;
using Environment;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class DangerScreen : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform _dangerScreenTransform;
        [SerializeField] private RectTransform _filterTransform;
        [SerializeField] private Image _background;

        [Header("Animation")]
        [SerializeField] private float _animDuration = .25f;
        [SerializeField] private AnimationCurve _dangerScreenScaleCurve;
        [SerializeField] private AnimationCurve _filterScaleCurve;
        [SerializeField] private AnimationCurve _backgroundColorCurve;
        [SerializeField] private Color _baseColor = Color.clear;
        [SerializeField] private Color _endColor = Color.red;


        [Header("Idle Animation")]
        [SerializeField] private float _idleAnimDuration = 1.5f;
        [Range(0f, .2f)]
        [SerializeField] private float _idleAnimRange = .1f;
        [SerializeField] private AnimationCurve _idleAnimCurve;

        private WaterContainer _waterContainer;
        private Sequence _sequence;

        private void Start()
        {
            _waterContainer = EnvironmentManager.Player.WaterContainer;
            _waterContainer.OnQuantityChanged.AddListener(UpdateView);
        }

        private void OnDestroy()
        {
            _waterContainer?.OnQuantityChanged.RemoveListener(UpdateView);
        }

        private void UpdateView(float arg0)
        {
            if (_sequence.IsActive()) _sequence.Kill();

            float target = 1 - (_waterContainer.Quantity / _waterContainer.MaxQuantity);

            _sequence = DOTween.Sequence();
            _sequence.Append(GetScaleTween(_dangerScreenTransform, _dangerScreenScaleCurve, target, _animDuration))
                .Join(GetScaleTween(_filterTransform, _filterScaleCurve, target, _animDuration))
                .Join(GetColor(target, _animDuration))
                .OnComplete(OnComplete)
                .SetLoops(0)
                .Play();
            return;

            void OnComplete()
            {
                float idleTarget = Mathf.Clamp01(target - _idleAnimRange);

                _sequence = DOTween.Sequence();
                _sequence.Append(GetScaleTween(_dangerScreenTransform, _dangerScreenScaleCurve, idleTarget, _idleAnimDuration))
                    .Join(GetScaleTween(_filterTransform, _filterScaleCurve, idleTarget, _idleAnimDuration))
                    .Join(GetColor(idleTarget, _idleAnimDuration))
                    .SetEase(_idleAnimCurve)
                    .SetLoops(-1)
                    .Play();
            }

            Tween GetScaleTween(RectTransform rectTransform, AnimationCurve curve, float time, float duration)
            {
                return rectTransform.DOScale(curve.Evaluate(time), duration);
            }

            Tween GetColor(float time, float duration)
            {
                return _background.DOColor(Color.Lerp(_baseColor, _endColor, _backgroundColorCurve.Evaluate(time)), _animDuration);
            }
        }
    }
}