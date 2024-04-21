using Cysharp.Threading.Tasks;
using TransitionManagement;
using UnityEngine;

namespace Environment
{
    public class RetrySceneCaller : MonoBehaviour
    {
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
            Async().Forget();
            return;

            async UniTaskVoid Async()
            {
                await _transition.Play(TransitionMode.In);
                EnvironmentManager.ResetWorld();
                await UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(0);
                await _transition.Play(TransitionMode.Out);

            }
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
    }
}