namespace AudioUtil.Runtime
{
    public interface IVolumeActions
    {
        void AddMusicVolume(float additiveValue);
        void AddSFXVolume(float additiveValue);
        void MuteMusic();
        void MuteSFX();
    }
}