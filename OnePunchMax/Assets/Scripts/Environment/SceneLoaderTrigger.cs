using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Environment
{
    public class SceneLoaderTrigger : Trigger
    {
        [SerializeField] private AssetReference _sceneToLoad;

        public AssetReference SceneToLoad { get => _sceneToLoad; set => _sceneToLoad = value; }

        protected override void Apply()
        {
            Async().Forget();

            async UniTaskVoid Async()
            {
                await EnvironmentManager.MoveMap(-transform.position);
                EnvironmentManager.S_LastSceneLoaded = SceneToLoad;
            }
        }
    }
}