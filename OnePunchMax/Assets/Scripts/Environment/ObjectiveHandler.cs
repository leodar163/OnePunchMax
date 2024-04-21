using UnityEngine;

namespace Environment
{
    public class ObjectiveHandler : MonoBehaviour
    {
        [SerializeField] private bool _lastObjective;
        [SerializeField] private ObjectivePart[] _objectiveParts;
        [SerializeField] private GameObject[] _explosions;

        private void Start()
        {
            EnvironmentManager.CampExploded += OnCampExploded;
            foreach (ObjectivePart part in _objectiveParts)
                part.Triggered += OnTriggered;
        }

        private void OnDestroy()
        {
            EnvironmentManager.CampExploded += OnCampExploded;
            foreach (ObjectivePart part in _objectiveParts)
                if (part != null)
                    part.Triggered -= OnTriggered;
        }

        private void OnTriggered(bool triggered)
        {
            foreach (ObjectivePart part in _objectiveParts)
                if (!part.IsTriggered)
                    return;

            if (_lastObjective)
            {
                EnvironmentManager.CompleteLastObjective();
                return;
            }

            EnvironmentManager.CompletedObjectives++;
        }

        private void OnCampExploded()
        {
            foreach (GameObject explosion in _explosions)
                explosion.SetActive(true);
        }
    }
}