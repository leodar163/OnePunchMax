using DG.Tweening;
using Inputs;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Ui
{
    public class HudController : MonoBehaviour
    {
        [SerializeField] private GameObject _hud;
        [SerializeField] private Image _background;
        [SerializeField] private RectTransform _arm;

        [Header("Appear Animation")]
        [SerializeField] private float _backgroundAppearDuration = .25f;
        [SerializeField] private float _backgroundOpacity = .6f;
        [SerializeField] private AnimationCurve _backgroundAppearCurve;
        [Space]
        [SerializeField] private Vector2 _armOutOfScreenPosition;
        [SerializeField] private float _armAppearDuration = .25f;
        [SerializeField] private AnimationCurve _armAppearCurve;

        [Header("Disappear Animation")]
        [SerializeField] private float _backgroundDisappearDuration = .25f;
        [SerializeField] private AnimationCurve _backgroundDisappearCurve;
        [Space]
        [SerializeField] private float _armDisappearDuration = .25f;
        [SerializeField] private AnimationCurve _armDisappearCurve;

        private Sequence _sequence;

        private void Awake()
        {
            _arm.transform.localPosition = _armOutOfScreenPosition;
            _background.color = Color.clear;
        }

        private void Start()
        {
            InputsUtility.MainControls.Actions.ShowUi.started += OnShowUiStarted;
            InputsUtility.MainControls.Actions.ShowUi.canceled += OnShowUiCancelled;
        }

        private void OnDestroy()
        {
            InputsUtility.MainControls.Actions.ShowUi.started -= OnShowUiStarted;
            InputsUtility.MainControls.Actions.ShowUi.canceled -= OnShowUiCancelled;
        }

        private void OnShowUiCancelled(InputAction.CallbackContext context)
        {
            UiManager.HudOpened?.Invoke(false);

            if (_sequence.IsActive()) _sequence.Kill();

            _sequence = DOTween.Sequence();
            _sequence
                .Append(
                    _background.DOFade(0, _backgroundDisappearDuration)
                    .SetEase(_backgroundDisappearCurve))
                .Join(
                    _arm.DOAnchorPos(_armOutOfScreenPosition, _armDisappearDuration)
                    .SetEase(_armDisappearCurve))
                .OnComplete(() => _hud.SetActive(false))
                .SetAutoKill()
                .Play();
        }

        private void OnShowUiStarted(InputAction.CallbackContext obj)
        {
            _hud.SetActive(true);
            UiManager.HudOpened?.Invoke(true);

            if (_sequence.IsActive()) _sequence.Kill();

            _sequence = DOTween.Sequence();
            _sequence
                .Append(
                    _background.DOFade(_backgroundOpacity, _backgroundAppearDuration)
                    .SetEase(_backgroundAppearCurve))
                .Join(
                    _arm.DOAnchorPos(Vector2.zero, _armAppearDuration)
                    .SetEase(_armAppearCurve))
                .SetAutoKill()
                .Play();
        }
    }
}
