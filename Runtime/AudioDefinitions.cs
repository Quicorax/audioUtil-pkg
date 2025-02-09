using System;
using System.Collections.Generic;
using UnityEngine;

namespace es.quicorax.audioUtil.Runtime
{
    [CreateAssetMenu(menuName = "Quicorax/AudioUtil/AudioDefinitions", fileName = "AudioDefinitions")]
    public class AudioDefinitions : ScriptableObject
    {
        [SerializeField] private List<AudioDefinition> _audioData;
        public List<AudioDefinition> AudioData => _audioData;
    }

    [Serializable]
    public class AudioDefinition
    {
        public string AudioKey;
        public AudioMode AudioMode;
        public AudioClip SingleAudioFile;
        public AudioClip[] MultipleAudioFiles;
        public float ForgetProgression;
    }
}