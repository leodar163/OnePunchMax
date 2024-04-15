using UnityEngine;

namespace Interactions
{
    public class Target : MonoBehaviour
    {
        public void Die()
        {
            Destroy(gameObject);
        }

        public void Burn()
        {
            
        }

        public void GetStunt()
        {
            
        }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.TryGetComponent(out IProjectile projectile))
            {
                projectile.OnHitTarget(this);
            }
        }
    }
}