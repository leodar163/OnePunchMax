using Triggers;
using UnityEngine;

namespace Environment
{
    public class ExplosionTrigger : MonoBehaviour, ITriggerable
    {
        private bool _triggered;

        private void Start()
        {
            EnvironmentManager.ObjectiveCompleted += OnObjectiveCompleted;
            EnvironmentManager.CampExploded += OnCampExploded;
        }

        private void OnDestroy()
        {
            EnvironmentManager.ObjectiveCompleted -= OnObjectiveCompleted;
            EnvironmentManager.CampExploded -= OnCampExploded;
        }

        private void OnCampExploded()
        {
            _triggered = false;
        }

        private void OnObjectiveCompleted()
        {
            _triggered = true;
        }

        public void TriggerEnter()
        {
            if (_triggered)
                EnvironmentManager.ExplodeCamp();
        }

        public void TriggerExit()
        {
            
        }
    }
}