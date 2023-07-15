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
            AudioPlayer.Do.Clear();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            AudioPlayer.Do.StopMusic("Music", 3);
        }   
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            AudioPlayer.Do.StopMusic("Music");
        }
         
        if (Input.GetKeyDown(KeyCode.D))
        {
            AudioPlayer.Do.StopMusic("Music", 1, () => Debug.Log("Fade Complete"));
        }
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            AudioPlayer.Do.StopAllMusics(2);
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
        
        if (Input.GetKeyDown(KeyCode.B))
        {
            AudioPlayer.Do.AddMasterVolume(1);
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            AudioPlayer.Do.AddMasterVolume(-1);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            AudioPlayer.Do.MuteMaster();
        }
    }
}