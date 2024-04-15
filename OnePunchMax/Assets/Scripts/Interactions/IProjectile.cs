using UnityEngine;

namespace Interactions
{
    public interface IProjectile
    {
        public Vector2 Direction { get; set; }
        public float Force { get; set; }
        
        public void OnHitTarget(Target target);
    }
}