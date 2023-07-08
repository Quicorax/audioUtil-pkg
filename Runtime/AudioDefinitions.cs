using System;
using System.Collections.Generic;
using UnityEngine;

namespace AudioUtil.Runtime
{
    [CreateAssetMenu(menuName = "Quicorax/AudioUtil/HardAudioDefinitions", fileName = "Hard Audio Definitions")]
    public class AudioDefinitions : ScriptableObject
    {
        [Serializable]
        public class AudioDefinition
        {
            public string AudioKey;
            public AudioClip AudioFile;
        }
    
        public List<AudioDefinition> Audios = new();
        public readonly Dictionary<string, AudioClip> SerializedAudios = new();
        
        public AudioDefinitions Serialize()
        {
            foreach (var audioDefinition in Audios)
            {
                SerializedAudios.Add(audioDefinition.AudioKey, audioDefinition.AudioFile);
            }

            return this;
        }
    }
}