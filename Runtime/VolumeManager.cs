using UnityEngine;
using UnityEngine.Audio;

namespace Services.Runtime.AudioService
{
    public class VolumeManager
    {
        private const string Master = "Master";
        private const string Music = "Music";
        private const string SFX = "SFX";
        
        private readonly AudioMixer _audioMixer;

        public VolumeManager(AudioMixer audioMixer)
        {
            _audioMixer = audioMixer;
        }

        public void AddMasterVolume(float additiveValue) => AddVolume(additiveValue, Master);
        public void AddMusicVolume(float additiveValue) => AddVolume(additiveValue, Music);
        public void AddSFXVolume(float additiveValue) => AddVolume(additiveValue, SFX);
        public bool MuteMaster() => MuteChannel(Master);
        public bool MuteMusic() => MuteChannel(Music);
        public bool MuteSFX() => MuteChannel(SFX);

        public void ConfigureInitialVolume()
        {
            ConfigureChannelVolume(Master);
            ConfigureChannelVolume(Music);
            ConfigureChannelVolume(SFX);
        }

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

        private bool MuteChannel(string conceptKey)
        {
            GenerateKeys(conceptKey, out var volumeKey, out var mutedKey, out var savedVolumeKey);

            var isMuted = PlayerPrefs.GetInt(mutedKey) == 1;

            _audioMixer.SetFloat(volumeKey, isMuted ? PlayerPrefs.GetFloat(savedVolumeKey) : -80);

            PlayerPrefs.SetInt(mutedKey, isMuted ? 0 : 1);

            return isMuted;
        }

        private void ConfigureChannelVolume(string conceptKey)
        {
            GenerateKeys(conceptKey, out var volumeKey, out var mutedKey, out var savedVolumeKey);

            _audioMixer.SetFloat(volumeKey, PlayerPrefs.GetInt(mutedKey) == 0 ? PlayerPrefs.GetFloat(savedVolumeKey) : -80);
        }

        private static void GenerateKeys(string conceptKey,
            out string volumeKey,
            out string mutedKey,
            out string savedVolumeKey)
        {
            volumeKey = string.Concat(conceptKey, "Volume");
            mutedKey = string.Concat(conceptKey, "Muted");
            savedVolumeKey = string.Concat("Saved", volumeKey);
        }
    }
}