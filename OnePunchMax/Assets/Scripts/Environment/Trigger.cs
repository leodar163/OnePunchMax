using UnityEngine;

namespace Environment
{
    public abstract class Trigger : MonoBehaviour
    {
        protected abstract void Apply();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Apply();
        }
    }
}