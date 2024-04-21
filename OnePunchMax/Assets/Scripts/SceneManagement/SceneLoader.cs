using Cysharp.Threading.Tasks;
using System;
using TransitionManagement;
using UnityEngine;
using UnityEngine.AddressableAssets;
using LoadSceneMode = UnityEngine.SceneManagement.LoadSceneMode;

namespace SceneManagement
{
    public static class SceneLoader
    {
        private static bool _transitionOutAllowed;
        private static bool _transitionInProgress;

        public static async UniTask LoadSceneAsync(AssetReference sceneRef, Transition transition, bool allowTransitionOut = true, LoadSceneMode loadSceneMode = LoadSceneMode.Single, Action callback = null)
        {
            try
            {
                if (_transitionInProgress)
                {
                    Debug.LogWarning("Already in transition, can't launch a new one");
                    return;
                }
                _transitionInProgress = true;

                await transition.Play(TransitionMode.In);
                await sceneRef.LoadSceneAsync(loadSceneMode);

                _transitionOutAllowed = allowTransitionOut;
                callback?.Invoke();
                await UniTask.WaitUntil(() => _transitionOutAllowed);

                await transition.Play(TransitionMode.Out);
                _transitionInProgress = false;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        public static async UniTask AllowTransitionOut()
        {
            _transitionOutAllowed = true;
            await UniTask.WaitWhile(() => _transitionInProgress);
        }
    }
}
