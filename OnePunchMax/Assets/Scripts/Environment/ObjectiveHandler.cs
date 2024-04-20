using UnityEngine;

namespace Environment
{
    public class ObjectiveHandler : MonoBehaviour
    {
        [SerializeField] private ObjectivePart[] _objectiveParts;

        private void Start()
        {
            foreach (ObjectivePart part in _objectiveParts)
                part.Triggered += OnTriggered;
        }

        private void OnDestroy()
        {
            foreach (ObjectivePart part in _objectiveParts)
                if (part != null)
                    part.Triggered -= OnTriggered;
        }

        private void OnTriggered(bool triggered)
        {
            foreach (ObjectivePart part in _objectiveParts)
                if (!part.IsTriggered)
                    return;

            EnvironmentManager.CompletedObjectives++;
        }
    }
}