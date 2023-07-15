using System;

namespace AudioUtil.Runtime
{
    public static class AudioPlayer
    {
        private class AudioActions : IAudioActions
        {
            public void PlaySFX(string key) => _audioNest.PlaySFX(key);
            public void PlayMusic(string key) => _audioNest.PlayMusic(key);
            public void StopMusic(string key, float fadeTime, Action onComplete) => _audioNest.StopMusic(key, fadeTime, onComplete);
            public void StopAllMusics(float fadeTime) => _audioNest.StopAllMusics(fadeTime);
            public void Clear() => _audioNest.Clear();
            
            public void AddMasterVolume(float additiveValue) => _audioNest.Volume.Do.AddMasterVolume(additiveValue);
            public void AddMusicVolume(float additiveValue) => _audioNest.Volume.Do.AddMusicVolume(additiveValue);
            public void AddSFXVolume(float additiveValue) => _audioNest.Volume.Do.AddSFXVolume(additiveValue);
            public void MuteMaster() => _audioNest.Volume.Do.MuteMaster();
            public void MuteMusic() => _audioNest.Volume.Do.MuteMusic();
            public void MuteSFX() => _audioNest.Volume.Do.MuteSFX();
        }

        public static readonly IAudioActions Do = new AudioActions();

        private static AudioNest _audioNest;

        public static void Initialize(AudioNest audioNest)
        {
            if (_audioNest == null)
            {
                _audioNest = audioNest;
            }
        }
    }
}