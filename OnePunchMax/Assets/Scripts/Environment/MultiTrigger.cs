using Triggers;
using UnityEngine;
using Utils;

namespace Environment
{
    public class MultiTrigger : MonoBehaviour, ITriggerable
    {
        [SerializeField] private UseLimiter _useLimiter;
        [Space]
        [SerializeField] private Object[] _callables;

        public void TriggerEnter()
        {
            if (!_useLimiter.Use()) return;

            foreach (Object callable in _callables)
            {
                ICallable c;
                GameObject obj = callable as GameObject;
                if (obj != null)
                {
                    if (!obj.TryGetComponent(out c))
                    {
                        Debug.LogWarning($"{obj.name} is not a callable");
                        continue;
                    }
                }
                else
                {
                    c = callable as ICallable;
                    if (c == null)
                    {
                        Debug.LogWarning("Not a callable");
                        continue;
                    }
                }
                c.Call();
            }
        }

        public void TriggerExit()
        {
            
        }
    }
}