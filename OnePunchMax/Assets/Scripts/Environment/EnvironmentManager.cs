using Behaviors;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
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

        public static int CompletedObjectives
        {
            get => _completedObjectives;
            set
            {
                _completedObjectives = value;
                ObjectiveCompleted?.Invoke();
            }
        }
        private static int _completedObjectives;

        public static PlayerController Player
        {
            get => _playerController;
            set
            {
                _playerController = value;
                _playerController.OnDie.AddListener(() => PlayerLost?.Invoke());
            }
        }
        private static PlayerController _playerController;
        
        private static List<Transform> _objectsToMove = new();
        private static int _currentLoaderId;

        private const float WATER_LOSE_DELAY = 1f;
        private const float WATER_LOSE_AMOUNT = 1f;

        private static CancellationTokenSource _waterTokenSource;
        private static CancellationToken WaterToken
        {
            get
            {
                if ( _waterTokenSource == null)
                    _waterTokenSource = new CancellationTokenSource();
                return _waterTokenSource.Token;
            }
        }

        public delegate void MoveEvent(Vector2 displacement);
        public static event MoveEvent MapMoved;

        public delegate void CompassEvent(SceneLoaderTrigger nextLoader);
        public static event CompassEvent CompassSet;

        public static event Action ObjectiveCompleted;
        public static event Action CampExploded;
        public static event Action LastObjectiveCompleted;
        public static event Action RetryAllowed;
        public static event Action PlayerLost;

        public static AudioSource CampAudio;
        public static AudioSource TravelAudio;
        public static AudioSource BossAudio;

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
    
        public static void EnterCamp()
        {
            TravelAudio.volume = 0f;
            CampAudio.volume = 1f;

            if (_waterTokenSource == null) return;

            _waterTokenSource.Cancel();
            _waterTokenSource.Dispose();
            _waterTokenSource = null;
        }

        public static void ExitCamp()
        {
            CampAudio.volume = 0f;
            TravelAudio.volume = 1f;

            LoseWater().Forget();
            return;

            async UniTaskVoid LoseWater()
            {
                while (true)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(WATER_LOSE_DELAY), cancellationToken: WaterToken);
                    Player.WaterContainer.Quantity -= WATER_LOSE_AMOUNT;
                }
            }
        }
    
        public static void ExplodeCamp()
        {
            CampExploded?.Invoke();
        }

        public static void CompleteLastObjective()
        {
            LastObjectiveCompleted?.Invoke();
        }

        public static void AllowRetry()
        {
            RetryAllowed?.Invoke();
        }
    }
}