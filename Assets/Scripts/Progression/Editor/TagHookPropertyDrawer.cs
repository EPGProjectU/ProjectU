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


        var tagNameProperty = property.FindPropertyRelative("tagName");

        // Refresh ProgressionManager to make sure ProgressionTags are cached
        ProgressionManager.SoftRefresh();

        var tagList = (from pTag in ProgressionManager.GetAllTags() select pTag.Name).ToList();

        // Get index of the current selected tag
        var index = tagList.FindIndex(t => t.Contains(tagNameProperty.stringValue));

        var defaultGUIColor = GUI.color;

        // If currently set tag name does not exist in current context make field yellow to indicate this
        if (index < 0)
            GUI.color = Color.yellow;

        EditorGUI.BeginChangeCheck();
        index = EditorGUI.Popup(position, index, tagList.ToArray());

        if (EditorGUI.EndChangeCheck())
            tagNameProperty.stringValue = tagList[index];

        EditorGUI.PropertyField(position, tagNameProperty, GUIContent.none);
        GUI.color = defaultGUIColor;

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}