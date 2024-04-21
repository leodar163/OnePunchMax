using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace TransitionManagement
{
    [CreateAssetMenu(fileName = "Fade Transition - ", menuName = "Transition/Fade")]
    public class FadeTransition : Transition
    {
        private const float ZERO = 0;
        private const float ONE = 1;

        [SerializeField] private Color _backgroundColor = Color.black;
        [SerializeField] private float _timeIn = .25f;
        [SerializeField] private AnimationCurve _animationCurveIn = new AnimationCurve(new (ZERO, ZERO), new (ONE, ONE));
        [SerializeField] private float _timeOut = .25f;
        [SerializeField] private AnimationCurve _animationCurveOut = new AnimationCurve(new(ZERO, ZERO), new(ONE, ONE));

        public override async UniTask Play(TransitionMode mode = TransitionMode.Complete)
        {
            switch (mode)
            {
                case TransitionMode.Complete:
                    await PlayIn();
                    await PlayOut();
                    break;

                case TransitionMode.In:
                    await PlayIn();
                    break;

                case TransitionMode.Out:
                    await PlayOut();
                    break;
            }
        }

        private async UniTask PlayIn() => await InternalPlay(ZERO, ONE, _timeIn, true, _animationCurveIn);
        private async UniTask PlayOut() => await InternalPlay(ONE, ZERO, _timeOut, false, _animationCurveOut);

        private async UniTask InternalPlay(float startA, float endA, float duration, bool endActive, AnimationCurve animationCurve)
        {
            Color backgroundColor = _backgroundColor;
            backgroundColor.a = startA;
            Image.color = backgroundColor;

            Canvas.gameObject.SetActive(true);
            Image.gameObject.SetActive(true);

            await Image.DOFade(endA, duration)
                .SetEase(animationCurve)
                .Play()
                .AsyncWaitForCompletion();

            Canvas.gameObject.SetActive(endActive);
            Image.gameObject.SetActive(endActive);
        }

        public override float Duration(TransitionMode mode = TransitionMode.Complete)
        {
            return mode switch
            {
                TransitionMode.In => _timeIn,
                TransitionMode.Out => _timeOut,
                _ => _timeIn + _timeOut
            };
        }
    }
}