using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Environment;

namespace Audios {
    public class AudioManager : MonoBehaviour
    {
        public AudioPlayer FightMusic;
        public AudioPlayer BossMusic;
        public AudioPlayer TravelMusic;
        public AudioPlayer DesertAmbience;

        void Awake()
        {
            DesertAmbience.FadeIn();
            EnvironmentManager.CampEntered+=OnCampEntered;
            EnvironmentManager.CampExited+=OnCampExited;
        }

        void OnDestroy()
        {
            EnvironmentManager.CampEntered-=OnCampEntered;
            EnvironmentManager.CampExited-=OnCampExited;
        }

        private void OnCampEntered(int campId){
            TravelMusic.FadeOut();
            if(campId < EnvironmentManager.LoaderTriggers.Count-1){
                FightMusic.FadeIn();
            }
            else {
                BossMusic.FadeIn();
            }
        }

        private void OnCampExited(int campId){
            if(campId < EnvironmentManager.LoaderTriggers.Count-1){
                FightMusic.FadeOut();
            }
            else {
                BossMusic.FadeOut();
            }
            TravelMusic.FadeIn();
        }
    }
}
