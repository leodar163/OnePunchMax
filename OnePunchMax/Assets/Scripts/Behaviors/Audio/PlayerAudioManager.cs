using System;
using UnityEngine;

namespace Behaviors.Audio
{
    public class PlayerAudioManager : HumanoidAudioManager
    {
        [SerializeField] private AudioClip[] _chargingClips;
        [SerializeField] private AudioClip[] _heavyAttackClips;
        [SerializeField] private AudioClip[] _hurtClips;
        [SerializeField] private AudioClip[] _healClips;

        [SerializeField] [HideInInspector] private PlayerController _playerController;

        private bool _lastChargingState;
        
        private void OnValidate()
        {
            if (_controller == null) return;
            if (_controller is not PlayerController pc)
            {
                Debug.LogWarning("Controller must be a PlayerController here");
                _controller = null;
            }
            else
            {
                _playerController = pc;
            }
        }
        
        protected override void Awake()
        {
            base.Awake();
            _playerController.onGetHealed.AddListener(() => PlayClipsGeneral(_healClips));
            _playerController.onGetHurt.AddListener(_ => PlayClipsGeneral(_hurtClips));
        }

        protected override void Update()
        {
            base.Update();
            ManageChargedSound();
        }

        private void ManageChargedSound()
        {
            if (_playerController.IsCharging && !_lastChargingState)
            {
                PlayClipsGeneral(_chargingClips);
            }

            if (!_playerController.IsCharging && _lastChargingState)
            {
                _generalPlayer.StopAudio();
            }

            _lastChargingState = _playerController.IsCharging;
        }
        
        protected override void PlayAttackClip(int attackType)
        {
            base.PlayAttackClip(attackType);
            if (attackType > 0)
            {
                PlayClipsGeneral(_heavyAttackClips);
            }
        }
    }
}