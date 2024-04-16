using System;
using UnityEngine;

namespace Interactions
{
    public class ObjectThrower : MonoBehaviour, IThrower
    {
        [SerializeField] private float _force;
        [SerializeField] private float _spineForce;
        [SerializeField] private float _range;
        
        public Vector2 Direction { get; set; }
        public float Force { get => _force; set => _force = value; }
        public float SpineForce { get => _spineForce; set => _spineForce = value; }
        public float Range { get => _range; set => _range = value; }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _range);
        }

        public void Throw(IThrowable throwable)
        {
            throwable.OnThrown(this);
        }
    }
}