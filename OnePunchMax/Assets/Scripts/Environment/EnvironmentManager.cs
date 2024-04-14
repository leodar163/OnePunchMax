using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Environment
{
    public static class EnvironmentManager
    {
        public static AssetReference S_LastSceneLoaded
        {
            get => s_lastSceneLoaded;
            set
            {
                if (s_lastSceneLoaded == value)
                    return;

                Async().Forget();

                async UniTaskVoid Async()
                {
                    s_lastSceneLoaded?.UnLoadScene();
                    s_lastSceneLoaded = value;
                    await s_lastSceneLoaded.LoadSceneAsync(LoadSceneMode.Additive);
                }
            }
        }

        private static AssetReference s_lastSceneLoaded;
        private static List<Transform> _objectsToMove = new();

        public delegate void MoveEvent(Vector2 displacement);
        public static event MoveEvent S_MapMoved;

        public static void SubscribeToMove(Transform transform)
        {
            _objectsToMove.Add(transform);
        }

        public static void UnsubscribeToMove(Transform transform)
        {
            _objectsToMove.Remove(transform);
        }

        public static async UniTask MoveMap(Vector2 displacement)
        {
            await UniTask.WaitForFixedUpdate();

            for (int i = 0; i < _objectsToMove.Count; i++)
                _objectsToMove[i].position += (Vector3)displacement;

            S_MapMoved?.Invoke(displacement);
        }
    }
}