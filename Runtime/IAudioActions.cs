namespace AudioUtil.Runtime
{
    public interface IAudioActions : IVolumeActions
    {
        void PlaySFX(string key);
        void PlayMusic(string key);
        void ClearAllAudio();
    }
}