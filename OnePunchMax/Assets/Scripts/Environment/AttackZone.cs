using Behaviors.Attack;
using Cysharp.Threading.Tasks;
using Detections;
using Interactions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Environment
{
    public class AttackZone : MonoBehaviour, IPositionnable
    {
        [SerializeField] private AttackData _attackData;
        [SerializeField] private float _attackDuration;
        [SerializeField] private bool _hideObjectAfterDuration;
        [SerializeField] private Collider2D _collider;

        private HashSet<ITarget> _hitTargets = new HashSet<ITarget>();

        public Vector3 Position => transform.position;

        private bool _destroyed;

        private void Awake()
        {
            _attackData.source = this;
        }

        private void Start()
        {
            EnvironmentManager.MapMoved += OnMapMoved;
        }

        private void OnDestroy()
        {
            _destroyed = true;
            EnvironmentManager.MapMoved -= OnMapMoved;
        }

        private void OnEnable()
        {
            if (_attackDuration <= 0) return;

            Async().Forget();
            return;

            async UniTaskVoid Async()
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_attackDuration));

                if (_destroyed) return;

                if (_hideObjectAfterDuration)
                {
                    gameObject.SetActive(false);
                    return;
                }
                _collider.enabled = false;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out ITarget target))
            {
                if (_hitTargets.Contains(target)) return;
                _hitTargets.Add(target);
                target.ReceiveAttack(_attackData);
            }
        }

        private void OnMapMoved(Vector2 displacement)
        {
            gameObject.SetActive(false);
        }
    }
}