using Cysharp.Threading.Tasks;
using Environment;
using TransitionManagement;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace SceneManagement
{
    public class InputSceneCaller : MonoBehaviour
    {
        [SerializeField] private AssetReference _sceneReference;
        [SerializeField] private Transition _transition;

        private bool _entered;

        private void Awake()
        {
            EnvironmentManager.CampEntered += OnCampEntered;
        }

        private void Update()
        {
            if (!Input.anyKeyDown) return;

            enabled = false;
            SceneLoader.LoadSceneAsync(_sceneReference, _transition, false, callback: OnSceneLoaded).Forget();
        }

        private void OnCampEntered(int obj)
        {
            EnvironmentManager.CampEntered -= OnCampEntered;
            _entered = true;
        }

        private void OnSceneLoaded()
        {
            Async().Forget();

            async UniTaskVoid Async()
            {
                await UniTask.WaitUntil(() => _entered);
                SceneLoader.AllowTransitionOut().Forget();
            }
        }
    }
}