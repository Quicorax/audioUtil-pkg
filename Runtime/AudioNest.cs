using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace es.quicorax.audioUtil.Runtime
{
    public class AudioNest : MonoBehaviour
    {
        private const int MaxAudioSources = 100;

        private AudioDefinitions _audioDefinitions;

        private readonly Dictionary<string, AudioSource> _activeMusics = new();

        private int _instancedAudioSources;
        private Tween _fadeTween;

        private bool _isMusicMuted;
        private bool _isSfxMuted;
        
        private int _currentProgressiveAudioClipIndex = -1;

        public AudioNest Initialize(AudioDefinitions audioDefinitions)
        {
            _audioDefinitions = audioDefinitions;
            
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

        public void PlayMusic(string clipKey, out AudioSource audioSource)
        {
            audioSource = null;

            if (ClipNotFound(clipKey) || MusicClipAlreadyPlaying(clipKey))
            {
                return;
            }

            audioSource = SetUpAudioSource(true, clipKey);

            if (audioSource is null)
            {
                return;
            }

            _activeMusics.Add(clipKey, audioSource);

            audioSource.volume = PlayerPrefs.GetFloat("MusicVolume");
            audioSource.mute = PlayerPrefs.GetInt("MusicMuted") == 1;

            audioSource.Play();
        }

        public void PlayMusicWithIntro(string introClipKey, string musicClipKey)
        {
            if (ClipNotFound(introClipKey) || ClipNotFound(musicClipKey))
            {
                return;
            }

            PlayMusic(introClipKey, out var audioSource);
            if (audioSource is null)
            {
                return;
            }

            audioSource.loop = false;
            StartCoroutine(QueueMusic(audioSource, musicClipKey));
        }

        public void StopMusicWithIntro(string introClipKey, string musicClipKey, float fadeTime = 0,
            Action onComplete = null)
        {
            StopAllCoroutines();
            StopMusic(introClipKey, fadeTime);
            StopMusic(musicClipKey, fadeTime, onComplete);
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
            if (!_activeMusics.TryGetValue(clipKey, out var audioSource))
            {
                onComplete?.Invoke();
                return;
            }

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

        public bool ToggleMuteMusic()
        {
            _isMusicMuted = !_isMusicMuted;

            MuteMusic(_isMusicMuted);
            return _isMusicMuted;
        }

        public bool ToggleMuteSFX()
        {
            _isSfxMuted = !_isSfxMuted;

            MuteSFX(_isSfxMuted);
            return _isSfxMuted;
        }

        public void ClearAudio()
        {
            StopAllCoroutines();

            _fadeTween?.Kill();
            _activeMusics.Clear();

            for (var index = 0; index < transform.childCount; index++)
            {
                Destroy(transform.GetChild(index).gameObject);
            }

            _instancedAudioSources = 0;
        }

        private AudioSource SetUpAudioSource(bool loop, string audioKey)
        {
            var audioSource = transform.GetComponentsInChildren<AudioSource>()
                .FirstOrDefault(source => !source.isPlaying) ?? CreateNewAudioSourceChildren();

            if (audioSource is not null)
            {
                audioSource.loop = loop;
                
                var data = _audioDefinitions.AudioData.Find(audioDefinition => audioDefinition.AudioKey == audioKey);
                audioSource.clip = GetClip(data);
            }

            return audioSource;
        }

        private AudioClip GetClip(AudioDefinition audioDefinition)
        {
            switch (audioDefinition.AudioMode)
            {
                case AudioMode.Random:
                    return audioDefinition.MultipleAudioFiles[Random.Range(0, audioDefinition.MultipleAudioFiles.Length)];
                case AudioMode.Progressive:
                    CancelInvoke();
                    _currentProgressiveAudioClipIndex++;
                    if (_currentProgressiveAudioClipIndex >= audioDefinition.MultipleAudioFiles.Length)
                    {
                        _currentProgressiveAudioClipIndex = 0;
                    }
                    Invoke(nameof(ForgetProgressiveAudioClipProgression), audioDefinition.ForgetProgression);
                    return audioDefinition.MultipleAudioFiles[_currentProgressiveAudioClipIndex];
                default:
                    return audioDefinition.SingleAudioFile;
            }
        }

        private void ForgetProgressiveAudioClipProgression()
        {
            _currentProgressiveAudioClipIndex = -1;
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
            if (_audioDefinitions.AudioData.Any(definition => definition.AudioKey == clipKey))
            {
                return false;
            }

            Debug.LogError($"quicorax.audioutil: Audio Definition with key: {clipKey} not found");   
            return true;
        }

        private bool MusicClipAlreadyPlaying(string clipKey)
        {
            if (!_activeMusics.ContainsKey(clipKey))
            {
                return false;
            }

            Debug.LogWarning($"quicorax.audioutil: Audio with key {clipKey} is already been played in loop mode");
            return true;
        }

        private void MuteMusic(bool mute)
        {
            foreach (var musicAudioSource in _activeMusics.Values)
            {
                musicAudioSource.mute = mute;
            }

            PlayerPrefs.SetInt("MusicMuted", mute ? 1 : 0);
        }

        private void MuteSFX(bool mute)
        {
            foreach (var sfxAudioSource in GetSfxAudioSources())
            {
                sfxAudioSource.mute = mute;
            }

            PlayerPrefs.SetInt("SFXMuted", mute ? 1 : 0);
        }

        private void SetInitialState()
        {
            _isMusicMuted = PlayerPrefs.GetInt("MusicMuted") == 1;
            _isSfxMuted = PlayerPrefs.GetInt("SFXMuted") == 1;
            MuteMusic(_isMusicMuted);
            MuteSFX(_isSfxMuted);

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
                SetMusicVolume(PlayerPrefs.GetFloat("MusicVolume"));
            }
        }

        private IEnumerable<AudioSource> GetSfxAudioSources()
        {
            return transform.GetComponentsInChildren<AudioSource>().Where(x => !x.loop);
        }

        private IEnumerator QueueMusic(AudioSource audioSource, string musicClipKey)
        {
            while (audioSource.isPlaying)
            {
                yield return null;
            }

            StopMusic(audioSource.clip.name, onComplete: () =>
            {
                PlayMusic(musicClipKey, out _);
            });
        }

        private void Awake() => DontDestroyOnLoad(gameObject);
        private void Start() => SetInitialState();
        private void OnDestroy() => _fadeTween.Kill();
    }
}