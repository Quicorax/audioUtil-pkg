using es.quicorax.audioUtil.Runtime;
using UnityEditor;
using UnityEngine;

namespace es.quicorax.audioUtil.Editor
{
    [CustomPropertyDrawer(typeof(AudioDefinition))]
    public class AudioDefinitionDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
        
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 1;

            var audioKey = property.FindPropertyRelative("AudioKey");
            var audioMode = property.FindPropertyRelative("AudioMode");
            var singleAudioFile = property.FindPropertyRelative("SingleAudioFile");
            var multipleAudioFiles = property.FindPropertyRelative("MultipleAudioFiles");
            var forgetProgression = property.FindPropertyRelative("ForgetProgression");

            var yOffset = position.y;
        
            EditorGUI.PropertyField(new Rect(position.x, yOffset, position.width, EditorGUIUtility.singleLineHeight), audioKey);
            yOffset += EditorGUIUtility.singleLineHeight + 2;

            EditorGUI.PropertyField(new Rect(position.x, yOffset, position.width, EditorGUIUtility.singleLineHeight), audioMode);
            yOffset += EditorGUIUtility.singleLineHeight + 2;

            var mode = (AudioMode)audioMode.enumValueIndex;

            switch (mode)
            {
                case AudioMode.Random:
                    EditorGUI.PropertyField(new Rect(position.x, yOffset, position.width, EditorGUIUtility.singleLineHeight), multipleAudioFiles, true);
                    break;
                case AudioMode.Progressive:
                    EditorGUI.PropertyField(new Rect(position.x, yOffset, position.width, EditorGUIUtility.singleLineHeight), forgetProgression, true);
                    yOffset += EditorGUIUtility.singleLineHeight + 2;
                    EditorGUI.PropertyField(new Rect(position.x, yOffset, position.width, EditorGUIUtility.singleLineHeight), multipleAudioFiles, true);
                    break;
                default:
                    EditorGUI.PropertyField(new Rect(position.x, yOffset, position.width, EditorGUIUtility.singleLineHeight), singleAudioFile, new GUIContent("Audio Clip"));
                    break;
            }

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var selectionMode = property.FindPropertyRelative("AudioMode");
            var multipleAudioFiles = property.FindPropertyRelative("MultipleAudioFiles");

            var height = 3 * (EditorGUIUtility.singleLineHeight + 2);
            var mode = (AudioMode)selectionMode.enumValueIndex;

            if (mode == AudioMode.Simple)
            {
                height += EditorGUIUtility.singleLineHeight + 2;
            }
            else
            {
                height += EditorGUI.GetPropertyHeight(multipleAudioFiles) + 2;
            }

            return height;
        }
    }
}
