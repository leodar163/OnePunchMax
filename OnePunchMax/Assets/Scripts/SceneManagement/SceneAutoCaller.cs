using Cysharp.Threading.Tasks;
using TransitionManagement;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace SceneManagement
{
    public class SceneAutoCaller : MonoBehaviour
    {
        [SerializeField] private AssetReference _sceneReference;
        [SerializeField] private Transition _transition;
        [SerializeField] private bool _allowTransitionOut;

        private void Start()
        {
            SceneLoader.LoadSceneAsync(_sceneReference, _transition, _allowTransitionOut).Forget();
        }
    }
}
