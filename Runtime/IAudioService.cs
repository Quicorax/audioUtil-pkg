using System;

namespace Services.Runtime.AudioService
{
    public interface IAudioService
    {
        void PlaySFX(string sfxKey);
        void PlayMusic(string musicKey);
        void StopMusic(string musicKey, float fadeTime = 0, Action onComplete = null);
        void TransitionMusic(string fromMusicKey, string toMusicKey, float fadeTime = 0);
        void StopAllMusics(float fadeTime);
        void SetMusicVolume(float finalVolume);
        void SetSFXVolume(float finalVolume);
        void MuteMusic(bool mute);
        void MuteSFX(bool mute);
        bool ToggleMuteMusic();
        bool ToggleMuteSFX();
        void ClearAudio();
    }
}