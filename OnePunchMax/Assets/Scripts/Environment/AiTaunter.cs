using Behaviors.AI;
using UnityEngine;
using Utils;

namespace Environment
{
    public class AiTaunter : MonoBehaviour, ICallable
    {
        [SerializeField] private UseLimiter _useLimiter;
        [Space]
        [SerializeField] private AIController[] _ais;

        public void Call()
        {
            if (!_useLimiter.Use()) return;

            foreach (AIController ai in _ais)
                if (!ai.IsDead)
                    ai.GoInAlertMode();
        }
    }
}