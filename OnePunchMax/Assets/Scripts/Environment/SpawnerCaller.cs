using Behaviors.AI;
using UnityEngine;
using Utils;

namespace Environment
{
    public class SpawnerCaller : MonoBehaviour, ICallable
    {
        [SerializeField] private UseLimiter _useLimiter;
        [Space]
        [SerializeField] private int _spawnQuantity = 1;
        [SerializeField] private AIController _original;

        public void Call()
        {
            if (!_useLimiter.Use()) return;

            for (int i = 0; i < _spawnQuantity; i++)
            {
                Vector3 displacement = _spawnQuantity == 1 ? Vector3.zero : Vector2.up.Rotate(360 * ((float)i / _spawnQuantity));
                AIController controller = Instantiate(_original, transform.position + displacement, Quaternion.identity, transform);
                controller.GoInAlertMode();
            }
        }
    }
}