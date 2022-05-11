using ProjectU.Core;
using UnityEditor;

namespace ProjectU
{
    [CustomEditor(typeof(TagList))]
    public class TagListEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();
            
            var tags = (serializedObject.targetObject as TagList)?.tagSet;
            
            string newTag;
            foreach (var tag in tags)
            {
                newTag = EditorGUILayout.TagField(tag);

                if (newTag == tag)
                    continue;

                // as iteration breaks after that, hash set can be edited inside the loop with no issues
                tags.Remove(tag);
                if (newTag != "Untagged")
                    tags.Add(newTag);
                break;
            }

            newTag = EditorGUILayout.TagField("");

            if (newTag != "" && newTag != "Untagged")
                tags.Add(newTag);
            
            // required for changes to be saved as non serializable property has been edited
            if (EditorGUI.EndChangeCheck())
                EditorUtility.SetDirty(serializedObject.targetObject);
                
            serializedObject.ApplyModifiedProperties();
        }
    }
}