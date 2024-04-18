using Environment;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class CompassView : MonoBehaviour
    {
        [SerializeField] private Sprite _armWithCompassSprite;
        [SerializeField] private Sprite _armWithoutCompassSprite;
        [SerializeField] private Image _armImage;
        [Space]
        [SerializeField] private Transform _hand;
        [SerializeField] private GameObject[] _compassParts;

        private SceneLoaderTrigger _target;

        private void Start()
        {
            EnvironmentManager.CompassSet += OnCompassSet;
            SetPartsActive(false);
        }

        private void OnDestroy()
        {
            EnvironmentManager.CompassSet -= OnCompassSet;
        }

        private void Update()
        {
            _hand.up = GetRotation;
        }

        private void OnCompassSet(SceneLoaderTrigger nextLoader)
        {
            _target = nextLoader;
            SetPartsActive(nextLoader != null);
        }

        private void SetPartsActive(bool active)
        {
            enabled = active;
            foreach (GameObject part in _compassParts)
                part.SetActive(enabled);
            _armImage.sprite = enabled ? _armWithCompassSprite : _armWithoutCompassSprite;
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