using System;

namespace AudioUtil.Runtime
{
    public interface IAudioActions : IVolumeActions
    {
        void PlaySFX(string key);
        void PlayMusic(string key);
        void StopMusic(string key, float fadeTime = 0, Action onComplete = null);
        
        void StopAllMusics(float fadeTime);
        void Clear();
    }
}