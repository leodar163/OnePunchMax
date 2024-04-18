using UnityEngine;

namespace Triggers
{
    public class TriggerCaller : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out ITriggerable triggerable))
                triggerable.Trigger();
        }
    }
}