using ProjectU.Core;
using UnityEditor;
using UnityEngine;

namespace ProjectU
{
    [CustomEditor(typeof(Tags))]
    public class TagsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(Tags.tagCollection)), true);
            
            serializedObject.ApplyModifiedProperties();
        }
    }

    
    [CustomPropertyDrawer(typeof(TagList))]
    public class TagListPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            EditorGUI.BeginChangeCheck();
            
            // Draw field name label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            
            var tags = (fieldInfo.GetValue(property.serializedObject.targetObject) as TagList)?.set;

            position.height /= tags.Count + 1;
            
            string newTag;
            foreach (var tag in tags)
            {
                newTag = EditorGUI.TagField(position, tag);
                position.y += position.height;
                
                if (newTag == tag)
                    continue;

                // as iteration breaks after that, hash set can be edited inside the loop with no issues
                tags.Remove(tag);
                if (newTag != "Untagged")
                    tags.Add(newTag);
                break;
            }

            newTag = EditorGUI.TagField(position, "");

            if (newTag != "" && newTag != "Untagged")
                tags.Add(newTag);
            
            if (EditorGUI.EndChangeCheck())
                EditorUtility.SetDirty(property.serializedObject.targetObject);

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var tags = (fieldInfo.GetValue(property.serializedObject.targetObject) as TagList)?.set;

            return EditorGUIUtility.singleLineHeight * (tags!.Count + 1);
        }
    }
}