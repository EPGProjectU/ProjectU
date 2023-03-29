using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Draws <see cref="TagHook"/> in inspector as a field of a GameObject
/// </summary>
[CustomPropertyDrawer(typeof(TagHook))]
public class TagHookPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw field name label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Remove indent
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;


        var tagProperty = property.FindPropertyRelative("<Tag>k__BackingField");
        var tag = tagProperty.objectReferenceValue as TagNode;

        ProgressionManager.ClearCache();

        var tagList = ProgressionManager.GetAllTags().OrderBy(tagNode => tagNode.Name).ToList();

        // Get index of the current selected tag
        var index = tagList.FindIndex(t => t.Equals(tag));

        var defaultGUIColor = GUI.color;

        // If currently set tag name does not exist in current context make field yellow to indicate this
        if (tag != null && index < 0)
        {
            GUI.color = Color.yellow;
            index = 0;
            tagList = tagList.Prepend(tag).ToList();
        }

        EditorGUI.BeginChangeCheck();
        index = EditorGUI.Popup(position, index < 0 ? -1 : index + 1, tagList.Select(tagNode => tagNode.Name).Prepend("-- Reset --").ToArray());
        
        if (EditorGUI.EndChangeCheck())
        {
            tagProperty.objectReferenceValue = index == 0 ? null : tagList[index - 1] as Object;
        }

        //EditorGUI.TextField(position, tag?.Name);

        GUI.color = defaultGUIColor;

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}