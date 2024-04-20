using System;
using System.Collections;
using Audios;
using UnityEngine;

namespace Behaviors.Audio
{
    public class HumanoidAudioManager : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] protected HumanoidController _controller;
        [SerializeField] private AudioPlayer _stepPlayer;
        [SerializeField] private AudioPlayer _attackPlayer;
        [SerializeField] protected AudioPlayer _generalPlayer;
        [Header("Settings")] 
        [SerializeField] [Min(0)] private float _stepIntervalTime;
        private IEnumerator _stepRoutine;
        [Header("Audios")] 
        [SerializeField] private AudioClip[] _stepsClips;
        [SerializeField] private AudioClip[] _dieClips;
        [SerializeField] private AudioClip[] _lightAttackClips;

        protected virtual void Awake()
        {
            _stepPlayer.SetClips(_stepsClips);
            _controller.OnDie.AddListener(() => {PlayClipsGeneral(_dieClips);});
            _controller.OnAttack.AddListener(PlayAttackClip);
        }

        protected virtual void Update()
        {
            ManageSteps();
        }

        protected void PlayClips(AudioPlayer player, AudioClip[] clips)
        {
            player.SetClips(clips);
            _stepPlayer.PlayAudioRandom();
        }
        
        protected void PlayClipsGeneral(AudioClip[] clips)
        {
            PlayClips(_generalPlayer, clips);
        }

        protected virtual void PlayAttackClip(int attackType)
        {
            if (attackType == 0)
            {
                PlayClips(_attackPlayer, _lightAttackClips);
            }
        }
        
        private void ManageSteps()
        {
            if (_controller.GetMovement().magnitude > 0)
            {
                if (_stepRoutine == null)
                {
                    _stepRoutine = StepRoutine();
                    StartCoroutine(_stepRoutine);
                }
            }
            else
            {
                if (_stepRoutine != null)
                {
                    StartCoroutine(_stepRoutine);
                    _stepRoutine = null;
                }
            }
        }
        
        private IEnumerator StepRoutine()
        {
            while (_controller.GetMovement().magnitude > 0)
            {
                print("Step");
                _stepPlayer.PlayAudioRandom();
                yield return new WaitForSeconds(_stepIntervalTime);   
            }
        }
    }
}