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

        public MixerManager Mixer;

        private AudioDefinitions _audioDefinitions;
        private AudioMixerGroup _musicMixer;
        private AudioMixerGroup _sfxMixer;

        private readonly Dictionary<string, AudioSource> _activeMusics = new();

        private int _instancedAudioSources;
        private Tween _fadeTween;

        public AudioNest Initialize(AudioDependencies data)
        {
            Mixer = new MixerManager(data.AudioMixer);
            _musicMixer = data.MusicMixer;
            _sfxMixer = data.SFXMixer;
            _audioDefinitions = data.AudioDefinitions;

            _audioDefinitions.Initialize();
            gameObject.name = "AudioManager";

            SetInitialVolume();

            return this;
        }

        private void SetInitialVolume()
        {
            if (PlayerPrefs.GetInt("InitialSFXVolumeSet") == 0)
            {
                PlayerPrefs.SetInt("InitialSFXVolumeSet", 1);
                SetSFXVolume(1f);
            }
            else
            {
                SetSFXVolume(PlayerPrefs.GetFloat("SFXVolume"));
            }

            if (PlayerPrefs.GetInt("InitialMusicVolumeSet") == 0)
            {
                PlayerPrefs.SetInt("InitialMusicVolumeSet", 1);
                SetMusicVolume(1f);
            }
            else
            {
                SetSFXVolume(PlayerPrefs.GetFloat("MusicVolume"));
            }
        }

        public void PlaySFX(string clipKey)
        {
            if (ClipNotFound(clipKey))
            {
                return;
            }

            var audioSource = SetUpAudioSource(false, clipKey, _sfxMixer);

            audioSource.volume = PlayerPrefs.GetFloat("SFXVolume");
            audioSource.Play();
        }

        public void PlayMusic(string clipKey)
        {
            if (ClipNotFound(clipKey) || MusicClipAlreadyPlaying(clipKey))
            {
                return;
            }

            var audioSource = SetUpAudioSource(true, clipKey, _musicMixer);

            if (audioSource is null)
            {
                return;
            }

            _activeMusics.Add(clipKey, audioSource);

            audioSource.volume = PlayerPrefs.GetFloat("MusicVolume");
            audioSource.Play();
        }

        public void SetSFXVolume(float volume)
        {
            foreach (var sfxAudioSource in transform.GetComponentsInChildren<AudioSource>().Where(x => !x.loop))
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

        private AudioSource SetUpAudioSource(bool loop, string clipKey, AudioMixerGroup mixer)
        {
            var audioSource = transform.GetComponentsInChildren<AudioSource>()
                .FirstOrDefault(source => !source.isPlaying) ?? CreateNewAudioSourceChildren();

            if (audioSource is not null)
            {
                audioSource.loop = loop;
                audioSource.clip = _audioDefinitions.SerializedAudios[clipKey];
                audioSource.outputAudioMixerGroup = mixer;
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

        private void Awake() => DontDestroyOnLoad(gameObject);
        private void Start() => Mixer.ConfigureInitialVolume();
        private void OnDestroy() => _fadeTween.Kill();
    }
}