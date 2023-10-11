using System;
using UnityEngine;

namespace Services.Runtime.AudioService
{
    public class AudioService : IAudioService
    {
        private readonly AudioNest _audioNest;
        
        public AudioService()
        {
            _audioNest = new GameObject().AddComponent<AudioNest>().Initialize(FetchDependencies());
        }
        
        public void PlaySFX(string sfxKey) => _audioNest.PlaySFX(sfxKey);
        public void PlayMusic(string musicKey) => _audioNest.PlayMusic(musicKey);
        public void StopMusic(string musicKey, float fadeTime = 0, Action onComplete = null) => _audioNest.StopMusic(musicKey, fadeTime, onComplete);
        public void TransitionMusic(string fromMusicKey, string toMusicKey, float fadeTime = 0) => _audioNest.StopMusic(fromMusicKey, fadeTime, ()=> PlayMusic(toMusicKey));
        public void StopAllMusics(float fadeTime) => _audioNest.StopAllMusics(fadeTime);
        public void AddMasterVolume(float additiveValue) => _audioNest.Volume.AddMasterVolume(additiveValue);
        public void AddMusicVolume(float additiveValue) => _audioNest.Volume.AddMusicVolume(additiveValue);
        public void AddSFXVolume(float additiveValue) => _audioNest.Volume.AddSFXVolume(additiveValue);
        public bool MuteMaster() => _audioNest.Volume.MuteMaster();
        public bool MuteMusic() => _audioNest.Volume.MuteMusic();
        public bool MuteSFX() => _audioNest.Volume.MuteSFX();
        public void ClearAudio() => _audioNest.ClearAudio();

        private AudioDependencies FetchDependencies()
        {
            var dependencies = Resources.Load<AudioDependencies>("AudioDependencies");

            if (dependencies == null)
            {
                Debug.LogError("No Audio dependencies defined in the Resources folder!");
            }
            
            return dependencies;
        }
    }
}