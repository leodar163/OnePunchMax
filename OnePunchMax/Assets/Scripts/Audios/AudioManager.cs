using Environment;
using UnityEngine;

namespace Audios
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioPlayer FightMusic;
        [SerializeField] private AudioPlayer BossMusic;
        [SerializeField] private AudioPlayer TravelMusic;

        private void Start()
        {
            EnvironmentManager.CampEntered += OnCampEntered;
            EnvironmentManager.CampExited += OnCampExited;
        }

        private void OnDestroy()
        {
            EnvironmentManager.CampEntered -= OnCampEntered;
            EnvironmentManager.CampExited -= OnCampExited;
        }

        private void OnCampEntered(int campId)
        {
            TravelMusic.FadeOut();
            Debug.Log(EnvironmentManager.LoaderTriggers.Count);

            if (EnvironmentManager.LoaderTriggers.Count == 0 || campId < EnvironmentManager.LoaderTriggers.Count-1)
            {
                FightMusic.FadeIn();
            }
            else
            {
                BossMusic.FadeIn();
            }
        }

        private void OnCampExited(int campId)
        {
            TravelMusic.FadeIn();

            if (campId < EnvironmentManager.LoaderTriggers.Count - 1)
            {
                FightMusic.FadeOut();
            }
            else
            {
                BossMusic.FadeOut();
            }
        }
    }
}
