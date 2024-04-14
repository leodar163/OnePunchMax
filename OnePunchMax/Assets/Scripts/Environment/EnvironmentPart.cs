using UnityEngine;

namespace Environment
{
    public class EnvironmentPart : MonoBehaviour
    {
        private void Start()
        {
            EnvironmentManager.SubscribeToMove(transform);
        }

        private void OnDestroy()
        {
            EnvironmentManager.UnsubscribeToMove(transform);
        }
    }
}