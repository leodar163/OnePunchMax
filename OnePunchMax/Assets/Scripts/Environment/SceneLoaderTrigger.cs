using Cysharp.Threading.Tasks;
using Triggers;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Environment
{
    public class SceneLoaderTrigger : MonoBehaviour, ITriggerable
    {
        public AssetReference SceneToLoad { get; set; }

        public void TriggerEnter()
        {
            Async().Forget();

            async UniTaskVoid Async()
            {
                await EnvironmentManager.MoveMap(-transform.position);
                EnvironmentManager.LastSceneLoaded = SceneToLoad;
            }
        }

        public void TriggerExit()
        {
            gameObject.SetActive(false);
        }
    }
}