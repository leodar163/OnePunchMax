using Inputs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ui
{
    public class HudController : MonoBehaviour
    {
        [SerializeField] private GameObject _hud;

        private void Start()
        {
            InputsUtility.MainControls.Actions.ShowUi.started += OnShowUiStarted;
            InputsUtility.MainControls.Actions.ShowUi.canceled += OnShowUiCancelled;
        }

        private void OnDestroy()
        {
            InputsUtility.MainControls.Actions.ShowUi.started -= OnShowUiStarted;
            InputsUtility.MainControls.Actions.ShowUi.canceled -= OnShowUiCancelled;
        }

        private void OnShowUiCancelled(InputAction.CallbackContext context)
        {
            _hud.SetActive(false);
            UiManager.HudOpened?.Invoke(false);
        }

        private void OnShowUiStarted(InputAction.CallbackContext obj)
        {
            _hud.SetActive(true);
            UiManager.HudOpened?.Invoke(true);
        }
    }
}
