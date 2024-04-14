using UnityEngine;

namespace Interactions
{
    public interface IThrower
    {
        public Vector2 Direction { get; set; }
        public float Force { get; set; }
        
        public void Throw(IThrowable throwable);
    }
}