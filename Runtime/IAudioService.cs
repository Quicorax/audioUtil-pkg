using System;

namespace es.quicorax.audioUtil.Runtime
{
    public interface IAudioService
    {
        void PlaySFX(string sfxKey);
        void PlayMusic(string musicKey);
        void PlayMusicWithIntro(string introKey, string musicKey);
        void StopMusicWithIntro(string introKey, string musicKey, float fadeTime = 0, Action onComplete = null);
        void StopMusic(string musicKey, float fadeTime = 0, Action onComplete = null);
        void TransitionMusic(string fromMusicKey, string toMusicKey, float fadeTime = 0);
        void StopAllMusics(float fadeTime);
        void SetMusicVolume(float finalVolume);
        void SetSFXVolume(float finalVolume);
        bool ToggleMuteMusic();
        bool ToggleMuteSFX();
        void ClearAudio();
    }
}