using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Audios
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioPlayer : MonoBehaviour
    {
        [SerializeField] protected AudioSource _audioSource;
        [Space] [SerializeField] private AudioClip[] _audios;
        [Space] 
        [SerializeField] private float _randomPitchMin;
        [SerializeField] private float _randomPitchMax;
        [Space] 
        [SerializeField] private float _fadeDuration = 0.5f;
        
        public float FadeDuration
        {
            get => _fadeDuration;
            set => _fadeDuration = value;
        }


        private IEnumerator _fadeRoutine;
        public float RandomPitchMin
        {
            get => _randomPitchMin;
            set => _randomPitchMin = value;
        }

        public float RandomPitchMax
        {
            get => _randomPitchMax;
            set => _randomPitchMax = value;
        }

        private void OnValidate()
        {
            if (!_audioSource) TryGetComponent(out _audioSource);
        }

        public void SetClips(AudioClip[] clips)
        {
            _audios = clips;
        }
        
        public void PlayAudio(AudioClip clip)
        {
            _audioSource.pitch = 1 + Random.Range(_randomPitchMin, _randomPitchMax);
            _audioSource.clip = clip;
            _audioSource.Play();
        }

        public void PlayAudio()
        {
            if(_audios.Length == 0) return;
            PlayAudio(_audios[0]);
        }

        public void PlayAudioRandom()
        {
            if(_audios.Length == 0) return;
            PlayAudio(_audios[Random.Range(0, _audios.Length)]);
        }

        public void StopAudio()
        {
            _audioSource.Stop();
        }
        
        #region Fading

        public void FadeOut()
        {
            FadeOut(_fadeDuration);
        }
        
        public void FadeOut(float duration)
        {
            Fade(duration, 0);
        }

        public void FadeIn()
        {
            FadeIn(_fadeDuration);
        }

        
        public void FadeIn(float duration)
        {
            Fade(duration, 1);
        }

        public void Fade(float targetVolume)
        {
            Fade(_fadeDuration, targetVolume);
        }
        
        public void Fade(float duration, float targetVolume)
        {
            if (_fadeRoutine != null)
            {
                StopCoroutine(_fadeRoutine);
                _fadeRoutine = null;
            }

            _fadeRoutine = FadeRoutine(duration, targetVolume);
            StartCoroutine(_fadeRoutine);
        }
        
        private IEnumerator FadeRoutine(float duration, float targetVolume)
        {
            float step = 1 / duration;
            float time = 0;
            float initialVolume = _audioSource.volume;

            while (Math.Abs(_audioSource.volume - targetVolume) > 0.00001f)
            {
                _audioSource.volume = Mathf.Lerp(initialVolume, targetVolume, time);
                yield return new WaitForEndOfFrame();
                time += step * Time.unscaledDeltaTime;
            }

            _fadeRoutine = null;
        }
        #endregion
    }
}