using Cinemachine;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Environment
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class EnvironmentCameraMover : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;

        CinemachineTransposer _transposer;

        private void Reset()
        {
            _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        }

        private void Awake()
        {
            _transposer = _virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        }

        private void Start()
        {
            EnvironmentManager.MapMoved += OnMapMoved;
        }

        private void OnDestroy()
        {
            EnvironmentManager.MapMoved -= OnMapMoved;
        }

        private void OnMapMoved(Vector2 displacement)
        {
            float x = _transposer.m_XDamping;
            float y = _transposer.m_YDamping;
            _transposer.m_XDamping = _transposer.m_YDamping = 0;
            Async().Forget();

            async UniTaskVoid Async()
            {
                await UniTask.Yield();
                _transposer.m_XDamping = x;
                _transposer.m_YDamping = y;
            }
        }
    }
}