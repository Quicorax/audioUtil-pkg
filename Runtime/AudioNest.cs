using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

namespace Services.Runtime.AudioService
{
    public class AudioNest : MonoBehaviour
    {
        private const int MaxAudioSources = 100;

        private AudioDefinitions _audioDefinitions;

        private readonly Dictionary<string, AudioSource> _activeMusics = new();

        private int _instancedAudioSources;
        private Tween _fadeTween;

        public AudioNest Initialize(AudioDefinitions audioDefinitions)
        {
            _audioDefinitions = audioDefinitions;

            _audioDefinitions.Initialize();
            gameObject.name = "AudioManager";

            return this;
        }

        public void PlaySFX(string clipKey)
        {
            if (ClipNotFound(clipKey))
            {
                return;
            }

            var audioSource = SetUpAudioSource(false, clipKey);

            audioSource.volume = PlayerPrefs.GetFloat("SFXVolume");
            audioSource.mute = PlayerPrefs.GetInt("SFXMuted") == 1;
            
            audioSource.Play();
        }

        public void PlayMusic(string clipKey)
        {
            if (ClipNotFound(clipKey) || MusicClipAlreadyPlaying(clipKey))
            {
                return;
            }

            var audioSource = SetUpAudioSource(true, clipKey);

            if (audioSource is null)
            {
                return;
            }

            _activeMusics.Add(clipKey, audioSource);

            audioSource.volume = PlayerPrefs.GetFloat("MusicVolume");
            audioSource.mute = PlayerPrefs.GetInt("MusicMuted") == 1;

            audioSource.Play();
        }

        public void SetSFXVolume(float volume)
        {
            foreach (var sfxAudioSource in GetSfxAudioSources())
            {
                sfxAudioSource.volume = volume;
            }

            PlayerPrefs.SetFloat("SFXVolume", volume);
        }

        public void SetMusicVolume(float volume)
        {
            foreach (var musicAudioSource in _activeMusics.Values)
            {
                musicAudioSource.volume = volume;
            }

            PlayerPrefs.SetFloat("MusicVolume", volume);
        }

        public void StopAllMusics(float fadeTime)
        {
            foreach (var musicAudio in _activeMusics)
            {
                StopMusic(musicAudio.Key, fadeTime);
            }
        }

        public void StopMusic(string clipKey, float fadeTime = 0, Action onComplete = null)
        {
            if (!_activeMusics.ContainsKey(clipKey))
            {
                return;
            }

            var audioSource = _activeMusics[clipKey];

            if (fadeTime > 0)
            {
                _fadeTween = DOTween.To(() => audioSource.volume, volume => audioSource.volume = volume, 0, fadeTime)
                    .OnComplete(() =>
                    {
                        ResetAudioSource(clipKey, audioSource);
                        onComplete?.Invoke();
                    });
            }
            else
            {
                ResetAudioSource(clipKey, audioSource);
            }
        }

        public bool MuteMusic()
        {
            var isMuted = PlayerPrefs.GetInt("MusicMuted") == 1;

            foreach (var musicAudioSource in _activeMusics.Values)
            {
                musicAudioSource.mute = isMuted;
            }

            PlayerPrefs.SetInt("MusicMuted", isMuted ? 0 : 1);

            return isMuted;
        }

        public bool MuteSFX()
        {
            var isMuted = PlayerPrefs.GetInt("SFXMuted") == 1;

            foreach (var sfxAudioSource in GetSfxAudioSources())
            {
                sfxAudioSource.mute = isMuted;
            }

            PlayerPrefs.SetInt("SFXMuted", isMuted ? 0 : 1);

            return isMuted;
        }

        public void ClearAudio()
        {
            _fadeTween?.Kill();
            _activeMusics.Clear();

            for (var index = 0; index < transform.childCount; index++)
            {
                Destroy(transform.GetChild(index).gameObject);
            }

            _instancedAudioSources = 0;
        }

        private AudioSource SetUpAudioSource(bool loop, string clipKey)
        {
            var audioSource = transform.GetComponentsInChildren<AudioSource>()
                .FirstOrDefault(source => !source.isPlaying) ?? CreateNewAudioSourceChildren();

            if (audioSource is not null)
            {
                audioSource.loop = loop;
                audioSource.clip = _audioDefinitions.SerializedAudios[clipKey];
            }

            return audioSource;
        }

        private AudioSource CreateNewAudioSourceChildren()
        {
            if (_instancedAudioSources >= MaxAudioSources)
            {
                return null;
            }

            var newAudioSource = new GameObject().AddComponent<AudioSource>();
            _instancedAudioSources++;

            newAudioSource.name = "AudioSource";
            newAudioSource.transform.parent = transform;
            newAudioSource.playOnAwake = false;

            return newAudioSource;
        }

        private void ResetAudioSource(string clipKey, AudioSource audioSource)
        {
            audioSource.mute = false;
            audioSource.clip = null;
            audioSource.volume = 1f;

            _activeMusics.Remove(clipKey);
        }

        private bool ClipNotFound(string clipKey)
        {
            if (!_audioDefinitions.SerializedAudios.ContainsKey(clipKey))
            {
                Debug.LogError($"Audio with key {clipKey} is not present in the AudioDefinitions");
                return true;
            }

            return false;
        }

        private bool MusicClipAlreadyPlaying(string clipKey)
        {
            if (!_activeMusics.ContainsKey(clipKey))
            {
                return false;
            }

            Debug.LogError($"Audio with key {clipKey} is already been played in loop mode");
            return true;
        }

        private void SetInitialVolume()
        {
            if (PlayerPrefs.GetInt("InitialSFXVolumeSet") == 0)
            {
                PlayerPrefs.SetInt("InitialSFXVolumeSet", 1);
                SetSFXVolume(0.5f);
            }
            else
            {
                SetSFXVolume(PlayerPrefs.GetFloat("SFXVolume"));
            }

            if (PlayerPrefs.GetInt("InitialMusicVolumeSet") == 0)
            {
                PlayerPrefs.SetInt("InitialMusicVolumeSet", 1);
                SetMusicVolume(0.5f);
            }
            else
            {
                SetSFXVolume(PlayerPrefs.GetFloat("MusicVolume"));
            }
        }

        private IEnumerable<AudioSource> GetSfxAudioSources()
        {
            return transform.GetComponentsInChildren<AudioSource>().Where(x => !x.loop);
        }

        private void Awake() => DontDestroyOnLoad(gameObject);
        private void Start() => SetInitialVolume();
        private void OnDestroy() => _fadeTween.Kill();
    }
}
