using UnityEngine;
using UnityEngine.Audio;

namespace Services.Runtime.AudioService
{
    [CreateAssetMenu(menuName = "Quicorax/AudioUtil/AudioDependencies", fileName = "AudioDependencies")]
    public class AudioDependencies : ScriptableObject
    {
        public AudioDefinitions AudioDefinitions;
        public AudioMixer AudioMixer;
        public AudioMixerGroup MusicMixer;
        public AudioMixerGroup SFXMixer;
    }
}