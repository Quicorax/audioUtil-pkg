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
            
            public void AddMusicVolume(float additiveValue)
            {
                if (PlayerPrefs.GetInt(AudioCommonConsts.MusicMuted) == 1)
                {
                    return;
                }
            
                var actualMusicVolume = PlayerPrefs.GetFloat(AudioCommonConsts.SavedMusicVolume) + additiveValue;

                _audioMixer.SetFloat("MusicVolume", actualMusicVolume);
            
                PlayerPrefs.SetFloat(AudioCommonConsts.SavedMusicVolume, actualMusicVolume);
            }

            public void AddSFXVolume(float additiveValue)
            {
                if (PlayerPrefs.GetInt(AudioCommonConsts.SFXMuted) == 1)
                {
                    return;
                }
            
                var actualSfxVolume = PlayerPrefs.GetFloat(AudioCommonConsts.SavedSFXVolume) + additiveValue;

                _audioMixer.SetFloat("SFXVolume", actualSfxVolume);
            
                PlayerPrefs.SetFloat(AudioCommonConsts.SavedSFXVolume, actualSfxVolume);
            }

            public void MuteMusic()
            {
                var muted = PlayerPrefs.GetInt(AudioCommonConsts.MusicMuted) == 1;

                _audioMixer.SetFloat("MusicVolume",
                    muted ? PlayerPrefs.GetFloat(AudioCommonConsts.SavedMusicVolume) : AudioCommonConsts.MutedAudioValue);

                PlayerPrefs.SetInt(AudioCommonConsts.MusicMuted, muted ? 0 : 1);
            }

            public void MuteSFX()
            {
                var muted = PlayerPrefs.GetInt(AudioCommonConsts.SFXMuted) == 1;

                _audioMixer.SetFloat("SFXVolume",
                    muted ? PlayerPrefs.GetFloat(AudioCommonConsts.SavedSFXVolume) : AudioCommonConsts.MutedAudioValue);

                PlayerPrefs.SetInt(AudioCommonConsts.SFXMuted, muted ? 0 : 1);
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
            _audioMixer.SetFloat("SFXVolume",
                PlayerPrefs.GetInt(AudioCommonConsts.SFXMuted) == 0
                    ? PlayerPrefs.GetFloat(AudioCommonConsts.SavedSFXVolume)
                    : AudioCommonConsts.MutedAudioValue);

            _audioMixer.SetFloat("MusicVolume",
                PlayerPrefs.GetInt(AudioCommonConsts.MusicMuted) == 0
                    ? PlayerPrefs.GetFloat(AudioCommonConsts.SavedMusicVolume)
                    : AudioCommonConsts.MutedAudioValue);
        }
    }
}