using UnityEngine;
using UnityEngine.Audio;

namespace AudioUtil.Runtime
{
    public class AudioUtilInitializer : MonoBehaviour
    {
        [SerializeField] private AudioDefinitions _audioDefinitions;
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private AudioMixerGroup _musicMixer;
        [SerializeField] private AudioMixerGroup _sfxMixer;

        void Awake()
        {
            AudioPlayer.Initialize(new GameObject().AddComponent<AudioNest>()
                .Initialize(_audioDefinitions.Serialize(), _audioMixer, _musicMixer, _sfxMixer));
        }
    }
}