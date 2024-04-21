using Cysharp.Threading.Tasks;
using TransitionManagement;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace SceneManagement
{
    public class InputSceneCaller : MonoBehaviour
    {
        [SerializeField] private AssetReference _sceneReference;
        [SerializeField] private Transition _transition;
        [SerializeField] private bool _allowTransitionOut;

        private void Update()
        {
            if (!Input.anyKeyDown) return;

            SceneLoader.LoadSceneAsync(_sceneReference, _transition, _allowTransitionOut).Forget();
            enabled = false;
        }
    }
}