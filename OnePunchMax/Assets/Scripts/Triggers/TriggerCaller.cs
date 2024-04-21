using UnityEngine;

namespace Triggers
{
    public class TriggerCaller : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out ITriggerable triggerable))
                triggerable.TriggerEnter();
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out ITriggerable triggerable))
                triggerable.TriggerExit();
        }
    }
}