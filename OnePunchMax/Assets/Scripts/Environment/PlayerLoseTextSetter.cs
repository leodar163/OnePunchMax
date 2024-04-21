using TMPro;
using UnityEngine;

namespace Environment
{
    public class PlayerLoseTextSetter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _label;
        [TextArea]
        [SerializeField] private string _text = "Perdu...";

        private void Start()
        {
            EnvironmentManager.PlayerLost += OnPlayerLost;
        }

        private void OnDestroy()
        {
            EnvironmentManager.PlayerLost -= OnPlayerLost;
        }

        private void OnPlayerLost()
        {
            _label.text = _text;
        }
    }
}