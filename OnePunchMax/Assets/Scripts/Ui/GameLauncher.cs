using Cysharp.Threading.Tasks;
using Environment;
using TransitionManagement;
using UnityEngine;

namespace Ui
{
    public class GameLauncher : MonoBehaviour
    {
        [SerializeField] private MapGenerator _mapGenerator;
        [SerializeField] private MapMemo _mapMemo;
        [SerializeField] private Transition _transition;

        private void Update()
        {
            if (!Input.anyKeyDown) return;

            enabled = false;

            Async().Forget();
            return;

            async UniTaskVoid Async()
            {
                await _transition.Play(TransitionMode.In);
                gameObject.SetActive(false);
                EnvironmentManager.LoaderTriggers = await _mapGenerator.Generate(_mapMemo);
                _transition.Play(TransitionMode.Out).Forget();
            }
        }
    }
}