using SceneManagement;

namespace Environment
{
    public class RetrySceneCaller : InputSceneCaller
    {
        private void Awake()
        {
            // TODO: si animation de fin de jeu, activer ça en RetryAllowed au lieu de LastObjectiveCompleted
            //EnvironmentManager.RetryAllowed += OnRetryAllowed;
            EnvironmentManager.LastObjectiveCompleted += OnRetryAllowed;
            EnvironmentManager.PlayerLost += OnRetryAllowed;
            gameObject.SetActive(false);
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