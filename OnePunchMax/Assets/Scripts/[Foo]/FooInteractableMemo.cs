using UnityEngine;

namespace Assets.Scripts._Foo_
{
    [CreateAssetMenu]
    public class FooInteractableMemo : ScriptableObject, IInteractable
    {
        public void Interact(Vector2 direction, Vector2 position)
        {
            // Do something
        }
    }
}