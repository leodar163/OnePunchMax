using UnityEngine;
using Utils.Serializable;

namespace _Foo_
{
    public class FooRequiringInterface : MonoBehaviour
    {
        [RequireInterface(typeof(IInteractable))]
        [SerializeField] private Object _interactable;

        private IInteractable Intercatable => _interactable as IInteractable;
    }
}