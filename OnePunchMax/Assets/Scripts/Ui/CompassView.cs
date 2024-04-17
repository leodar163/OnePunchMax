using Environment;
using UnityEngine;

namespace Ui
{
    public class CompassView : MonoBehaviour
    {
        [SerializeField] private Transform _hand;

        private SceneLoaderTrigger _target;

        private void Start()
        {
            EnvironmentManager.CompassSet += OnCompassSet;
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            EnvironmentManager.CompassSet -= OnCompassSet;
        }

        private void OnCompassSet(SceneLoaderTrigger nextLoader)
        {
            _target = nextLoader;
            gameObject.SetActive(_target != null);
        }

        private void Update()
        {
            _hand.up = GetRotation;
        }

        private Vector2 GetRotation
        {
            get
            {
                if (_target == null)
                    return Vector2.up;

                return (_target.transform.position - EnvironmentManager.Player.Position).normalized;
            }
        }
    }
}