namespace AudioUtil.Runtime
{
    public static class AudioPlayer
    {
        private class AudioActions : IAudioActions
        {
            public void PlaySFX(string key) => _audioNest.TryPlayAudio(key, false);
            public void PlayMusic(string key) => _audioNest.TryPlayAudio(key, true);
            public void ClearAllAudio() => _audioNest.ClearAllAudios();
            public void AddMusicVolume(float additiveValue) => _audioNest.Volume.Do.AddMusicVolume(additiveValue);
            public void AddSFXVolume(float additiveValue) => _audioNest.Volume.Do.AddSFXVolume(additiveValue);
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