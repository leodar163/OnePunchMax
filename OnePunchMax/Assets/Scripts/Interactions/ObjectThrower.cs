using UnityEngine;

namespace Interactions
{
    public class ObjectThrower : MonoBehaviour, IThrower
    {
        [SerializeField] private float _force;
        
        public Vector2 Direction { get; set; }
        public float Force { get => _force; set => _force = value; }

        public void Throw(IThrowable throwable)
        {
            throwable.OnThrown(this);
        }
    }
}