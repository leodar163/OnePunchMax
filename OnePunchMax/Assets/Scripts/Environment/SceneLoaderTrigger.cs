using Cysharp.Threading.Tasks;
using Triggers;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Environment
{
    public class SceneLoaderTrigger : MonoBehaviour, ITriggerable
    {
        [SerializeField] private AssetReference _sceneToLoad;

        public AssetReference SceneToLoad { get => _sceneToLoad; set => _sceneToLoad = value; }

        public void Trigger()
        {
            Async().Forget();

            async UniTaskVoid Async()
            {
                await EnvironmentManager.MoveMap(-transform.position);
                EnvironmentManager.LastSceneLoaded = SceneToLoad;
            }
        }
    }
}