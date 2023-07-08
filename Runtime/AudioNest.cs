using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

namespace AudioUtil.Runtime
{
    public class AudioNest : MonoBehaviour
    {
        public VolumeManager Volume;
        
        private AudioDefinitions _audioDefinitions;
        private AudioMixerGroup _musicMixer;
        private AudioMixerGroup _sfxMixer;

        private readonly List<string> _activeLoopingAudios = new();
        private readonly List<AudioSource> _activeAudioSources = new();

        public AudioNest Initialize(AudioDefinitions audioDefinitions, AudioMixer audioMixer,
            AudioMixerGroup musicMixer, AudioMixerGroup sfxMixer)
        {
            Volume = new VolumeManager(audioMixer);

            _audioDefinitions = audioDefinitions;
            _musicMixer = musicMixer;
            _sfxMixer = sfxMixer;

            gameObject.name = "AudioManager";
            return this;
        }

        public void TryPlayAudio(string clipKey, bool loop)
        {
            if (!_audioDefinitions.SerializedAudios.ContainsKey(clipKey))
            {
                Debug.LogError($"Audio with key {clipKey} is not present in the AudioDefinitions");
                return;
            }

            if (loop && _activeLoopingAudios.Contains(clipKey))
            {
                Debug.LogError($"Audio with key {clipKey} is already been played in loop mode");
                return;
            }

            PlayAudio(clipKey, loop);
        }

        private void PlayAudio(string clipKey, bool loop)
        {
            var audioSource = transform.GetComponentsInChildren<AudioSource>()
                .FirstOrDefault(source => !source.isPlaying) ?? CreateNewAudioSourceChildren();

            AudioMixerGroup mixerToAssign;
            if (loop)
            {
                _activeLoopingAudios.Add(clipKey);
                mixerToAssign = _musicMixer;
            }
            else
            {
                mixerToAssign = _sfxMixer;
            }

            audioSource.loop = loop;
            audioSource.clip = _audioDefinitions.SerializedAudios[clipKey];
            audioSource.outputAudioMixerGroup = mixerToAssign;

            audioSource.Play();
        }

        private AudioSource CreateNewAudioSourceChildren()
        {
            var newAudioSource = new GameObject().AddComponent<AudioSource>();
            newAudioSource.name = "AudioSource";
            newAudioSource.transform.parent = transform;
            newAudioSource.playOnAwake = false;

            _activeAudioSources.Add(newAudioSource);

            return newAudioSource;
        }

        public void ClearAllAudios()
        {
            _activeLoopingAudios.Clear();

            for (var index = 0; index < transform.childCount; index++)
            {
                Destroy(transform.GetChild(index).gameObject);
            }
        }

        private void Awake() => DontDestroyOnLoad(gameObject);
        private void Start() => Volume.ConfigureInitialVolume();
    }
}
