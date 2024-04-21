using Behaviors.Attack;
using Interactions;
using UnityEngine;

namespace Environment
{
    public class AttackSpawner : MonoBehaviour, ITarget
    {
        [SerializeField] private bool _destroyAfterSpawning;
        [SerializeField] private Sprite _destroyedSprite;
        [SerializeField] private SpriteRenderer _renderer;
        [Space]
        [SerializeField] private AttackZone _spawnedAttack;

        private bool _destroyed;

        public Vector3 Position => transform.position;

        public void ReceiveAttack(AttackData data)
        {
            ReceiveAttack();
        }

        public void ReceiveAttack()
        {
            if (_destroyed) return;
            _destroyed = true;

            Instantiate(_spawnedAttack, transform.position, Quaternion.identity);

            if (_destroyAfterSpawning)
            {
                gameObject.SetActive(false);
                return;
            }
            _renderer.sprite = _destroyedSprite;
        }
    }
}