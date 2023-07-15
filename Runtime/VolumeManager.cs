using UnityEngine;
using UnityEngine.Audio;

namespace AudioUtil.Runtime
{
    public class VolumeManager
    {
        public class AudioActions : IVolumeActions
        {
            private readonly AudioMixer _audioMixer;

            public AudioActions(AudioMixer audioMixer)
            {
                _audioMixer = audioMixer;
            }

            public void AddMasterVolume(float additiveValue) => AddVolume(additiveValue, "Master");
            public void AddMusicVolume(float additiveValue) => AddVolume(additiveValue, "Music");
            public void AddSFXVolume(float additiveValue) => AddVolume(additiveValue, "SFX");

            public void MuteMaster() => MuteChannel("Master");
            public void MuteMusic() => MuteChannel("Music");
            public void MuteSFX() => MuteChannel("SFX");

            private void AddVolume(float additiveValue, string conceptKey)
            {
                GenerateKeys(conceptKey, out var volumeKey, out var mutedKey, out var savedVolumeKey);

                if (PlayerPrefs.GetInt(mutedKey) == 1)
                {
                    return;
                }

                var actualMasterVolume = PlayerPrefs.GetFloat(savedVolumeKey) + additiveValue;
                _audioMixer.SetFloat(volumeKey, actualMasterVolume);

                PlayerPrefs.SetFloat(savedVolumeKey, actualMasterVolume);
            }

            private void MuteChannel(string conceptKey)
            {
                GenerateKeys(conceptKey, out var volumeKey, out var mutedKey, out var savedVolumeKey);

                var isMuted = PlayerPrefs.GetInt(mutedKey) == 1;

                _audioMixer.SetFloat(volumeKey,
                    isMuted ? PlayerPrefs.GetFloat(savedVolumeKey) : AudioCommonConsts.MutedAudioValue);

                PlayerPrefs.SetInt(mutedKey, isMuted ? 0 : 1);
            }
        }

        public readonly AudioActions Do;
        private readonly AudioMixer _audioMixer;

        public VolumeManager(AudioMixer audioMixer)
        {
            _audioMixer = audioMixer;
            Do = new AudioActions(audioMixer);
        }

        public void ConfigureInitialVolume()
        {
            ConfigureChannelVolume("Master");
            ConfigureChannelVolume("Music");
            ConfigureChannelVolume("SFX");
        }

        private void ConfigureChannelVolume(string conceptKey)
        {
            GenerateKeys(conceptKey, out var volumeKey, out var mutedKey, out var savedVolumeKey);

            _audioMixer.SetFloat(volumeKey,
                PlayerPrefs.GetInt(mutedKey) == 0
                    ? PlayerPrefs.GetFloat(savedVolumeKey)
                    : AudioCommonConsts.MutedAudioValue);
        }

        private static void GenerateKeys(string conceptKey, 
            out string volumeKey, out string mutedKey, out string savedVolumeKey)
        {
            volumeKey = string.Concat(conceptKey, "Volume");
            mutedKey = string.Concat(conceptKey, "Muted");
            savedVolumeKey = string.Concat("Saved", volumeKey);
        }
    }
}