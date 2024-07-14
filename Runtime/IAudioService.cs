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
        void AddMusicVolume(float finalVolume);
        void AddSFXVolume(float finalVolume);
        bool MuteMaster();
        bool MuteMusic();
        bool MuteSFX();
        void ClearAudio();
    }
}