using SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine;
using TransitionManagement;
using Cysharp.Threading.Tasks;

namespace Environment
{
    public class RetrySceneCaller : MonoBehaviour
    {
        [SerializeField] private AssetReference _sceneReference;
        [SerializeField] private Transition _transition;

        private void Awake()
        {
            // TODO: si animation de fin de jeu, activer ça en RetryAllowed au lieu de LastObjectiveCompleted
            //EnvironmentManager.RetryAllowed += OnRetryAllowed;
            EnvironmentManager.LastObjectiveCompleted += OnRetryAllowed;
            EnvironmentManager.PlayerLost += OnRetryAllowed;
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (!Input.anyKeyDown) return;

            enabled = false;
            SceneLoader.LoadSceneAsync(_sceneReference, _transition, false, callback: OnSceneLoaded).Forget();
        }

        private void OnDestroy()
        {
            //EnvironmentManager.RetryAllowed -= OnRetryAllowed;
            EnvironmentManager.LastObjectiveCompleted -= OnRetryAllowed;
            EnvironmentManager.PlayerLost -= OnRetryAllowed;
        }

        private void OnRetryAllowed()
        {
            gameObject.SetActive(true);
        }

        private void OnSceneLoaded()
        {
            EnvironmentManager.ResetWorld();
            SceneLoader.AllowTransitionOut().Forget();
        }
    }
}