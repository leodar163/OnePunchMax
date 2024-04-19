using Behaviors;
using DG.Tweening;
using Environment;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class WaterView : MonoBehaviour
    {
        [SerializeField] private Image _waterImage;

        [Header("Animation")]
        [SerializeField] private float _animDuration = .25f;
        [SerializeField] private AnimationCurve _animCurve;

        [Header("Blink Animation")]
        [SerializeField] private int _blinkAmount = 2;
        [SerializeField] private AnimationCurve _blinkCurve;
        [SerializeField] private Color _defaultColor = Color.white;
        [SerializeField] private Color _downColor = Color.red;
        [SerializeField] private Color _upColor = Color.green;

        private WaterContainer _waterContainer;
        private Tween _tween;

        private void Start()
        {
            _waterImage.fillAmount = 1;
            _waterImage.color = _defaultColor;
            _waterContainer = EnvironmentManager.Player.WaterContainer;
            _waterContainer.OnQuantityChanged.AddListener(UpdateView);
        }

        private void OnDestroy()
        {
            _waterContainer?.OnQuantityChanged.RemoveListener(UpdateView);
        }

        private void UpdateView(float arg0)
        {
            if (_tween.IsActive()) _tween.Kill();

            float target = _waterContainer.Quantity / _waterContainer.MaxQuantity;
            float current = _waterImage.fillAmount;
            bool down = target < current;
            Color color = down ? _downColor : _upColor;
            float currentTime = 0;
            float blinkDelay = _animDuration / _blinkAmount;

            _tween = DOVirtual.Float(current, target, _animDuration, 
                f =>
                {
                    _waterImage.color = GetColor(f);
                    _waterImage.fillAmount = f;
                })
                .SetEase(_animCurve)
                .OnComplete(() =>
                { 
                    _waterImage.color = _defaultColor;
                    _waterImage.fillAmount = target;
                })
                .Play();
            return;

            Color GetColor(float time)
            {
                currentTime = ((currentTime + Time.deltaTime) % blinkDelay) / blinkDelay;
                float curveValue = _blinkCurve.Evaluate(currentTime);
                return Color.Lerp(_defaultColor, color, curveValue);
            }
        }
    }
}