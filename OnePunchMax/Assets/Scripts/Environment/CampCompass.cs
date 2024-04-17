using Interactions;
using UnityEngine;

namespace Environment
{
    public class CampCompass : MonoBehaviour, IInteractable
    {
        public Vector3 Position => transform.position;

        public void OnHover()
        {
            // TODO: Sign "TAKE THAT COMPASS M*****F***R"
        }

        public void OnInteract(IInteractor interactor)
        {
            EnvironmentManager.SetCompass(transform.position);
            gameObject.SetActive(false);
        }
    }
}