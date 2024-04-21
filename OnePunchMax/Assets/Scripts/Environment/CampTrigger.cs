using Triggers;
using UnityEngine;

namespace Environment
{
    public class CampTrigger : MonoBehaviour, ITriggerable
    {
        public void TriggerEnter()
        {
            EnvironmentManager.EnterCamp();
        }

        public void TriggerExit()
        {
            EnvironmentManager.ExitCamp();
        }
    }
}