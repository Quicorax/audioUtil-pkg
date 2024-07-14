using System;
using System.Collections.Generic;
using UnityEngine;

namespace Services.Runtime.AudioService
{
    [CreateAssetMenu(menuName = "Quicorax/AudioUtil/AudioDefinitions", fileName = "AudioDefinitions")]
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

        public void Initialize()
        {
            foreach (var audioDefinition in Audios)
            {
                SerializedAudios.Add(audioDefinition.AudioKey, audioDefinition.AudioFile);
            }
        }
    }
}