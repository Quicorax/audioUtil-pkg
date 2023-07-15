namespace AudioUtil.Runtime
{
    public interface IVolumeActions
    {
        void AddMasterVolume(float additiveValue);
        void AddMusicVolume(float additiveValue);
        void AddSFXVolume(float additiveValue);
        
        void MuteMaster();
        void MuteMusic();
        void MuteSFX();
    }
}