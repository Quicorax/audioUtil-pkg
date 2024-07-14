using System;
using UnityEngine;

namespace Services.Runtime.AudioService
{
    public class AudioService : IAudioService
    {
        private readonly AudioNest _audioNest;
        private bool _isReady;
        
        public AudioService()
        {
            _audioNest = new GameObject().AddComponent<AudioNest>();
    
            var dependencies = Resources.LoadAsync("Audio/AudioDependencies");
            dependencies.completed += _ => SetDependencies(dependencies);
        }
    
        public void PlaySFX(string sfxKey) => IsReady(()=> _audioNest.PlaySFX(sfxKey));
        public void PlayMusic(string musicKey) => IsReady(()=> _audioNest.PlayMusic(musicKey));
        public void StopMusic(string musicKey, float fadeTime = 0, Action onComplete = null) => IsReady(()=> _audioNest.StopMusic(musicKey, fadeTime, onComplete));
        public void TransitionMusic(string fromMusicKey, string toMusicKey, float fadeTime = 0) => IsReady(()=> _audioNest.StopMusic(fromMusicKey, fadeTime, () => PlayMusic(toMusicKey)));
        public void StopAllMusics(float fadeTime) => IsReady(()=> _audioNest.StopAllMusics(fadeTime));
        public void AddMusicVolume(float finalVolume) => IsReady(()=> _audioNest.SetMusicVolume(finalVolume));
        public void AddSFXVolume(float finalVolume) => IsReady(()=> _audioNest.SetSFXVolume(finalVolume));
        public void ClearAudio() => IsReady(()=> _audioNest.ClearAudio());
        public bool MuteMaster() => IsReady(() => _audioNest.Volume.MuteMaster());
        public bool MuteMusic() => IsReady(() => _audioNest.Volume.MuteMusic());
        public bool MuteSFX() => IsReady(() => _audioNest.Volume.MuteSFX());
    
        private void SetDependencies(ResourceRequest asset)
        {
            if (asset == null)
            {
                Debug.LogError("No Audio dependencies defined in the Resources folder!");
            }
    
            _audioNest.Initialize(asset?.asset as AudioDependencies);
            _isReady = true;
        }
    
        private void IsReady(Action onReady)
        {
            if (_isReady)
            {
                onReady.Invoke();
                return;
            }
    
            Debug.LogWarning("AudioService is not ready. Skipped call");
        }
        
        private T IsReady<T>(Func<T> onReady)
        {
            if (_isReady)
            {
               return onReady.Invoke();
            }
    
            Debug.LogWarning("AudioService is not ready. Skipped call");
    
            return default;
        }
    }
}