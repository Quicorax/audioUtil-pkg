using AudioUtil.Runtime;
using UnityEngine;

public class AudioSampleCaller : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            AudioPlayer.Do.PlayMusic("Music");
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            AudioPlayer.Do.PlaySFX("Hit");
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            AudioPlayer.Do.ClearAllAudio();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            AudioPlayer.Do.AddMusicVolume(1);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            AudioPlayer.Do.AddMusicVolume(-1);
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            AudioPlayer.Do.MuteMusic();
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            AudioPlayer.Do.AddSFXVolume(1);
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            AudioPlayer.Do.AddSFXVolume(-1);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            AudioPlayer.Do.MuteSFX();
        }
    }
}