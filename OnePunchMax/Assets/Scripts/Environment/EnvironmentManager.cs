using Behaviors;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Environment
{
    public static class EnvironmentManager
    {
        private static readonly string NO_LOADER_ERROR = "There is no SceneLoaderTrigger at id {0}";

        public static AssetReference LastSceneLoaded
        {
            get => _lastSceneLoaded;
            set
            {
                if (_lastSceneLoaded == value)
                    return;

                Async().Forget();

                async UniTaskVoid Async()
                {
                    _lastSceneLoaded?.UnLoadScene();
                    _lastSceneLoaded = value;
                    await _lastSceneLoaded.LoadSceneAsync(LoadSceneMode.Additive);
                }
            }
        }
        private static AssetReference _lastSceneLoaded;

        public static SceneLoaderTrigger CurrentLoader => LoaderTriggers[_currentLoaderId];
        public static List<SceneLoaderTrigger> LoaderTriggers { get; set; }

        public static PlayerController Player { get; set; }
        
        private static List<Transform> _objectsToMove = new();
        private static int _currentLoaderId;

        public delegate void MoveEvent(Vector2 displacement);
        public static event MoveEvent MapMoved;

        public delegate void CompassEvent(SceneLoaderTrigger nextLoader);
        public static event CompassEvent CompassSet;

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

            MapMoved?.Invoke(displacement);
        }

        public static void SetCompassNull()
        {
            CompassSet?.Invoke(null);
        }
    
        public static void SetCompass(Vector3 position)
        {
            int nextLoader = GetNearestLoader(position) + 1;
            if (nextLoader >= LoaderTriggers.Count)
            {
                Debug.LogWarning(string.Format(NO_LOADER_ERROR, nextLoader));
                return;
            }

            _currentLoaderId = nextLoader;
            CompassSet?.Invoke(LoaderTriggers[nextLoader]);
        }

        private static int GetNearestLoader(Vector3 postion)
        {
            int index = 0;
            float distance = float.MaxValue;

            for (int i = 0; i < LoaderTriggers.Count; i++)
            {
                float currentDistance = Vector3.Distance(postion, LoaderTriggers[i].transform.position);
                if (currentDistance >= distance)
                    continue;

                distance = currentDistance;
                index = i;
            }

            return index;
        }
    }
}