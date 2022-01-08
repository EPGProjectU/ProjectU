using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(TagHook))]
public class TagHookPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;


        var tagNameProperty = property.FindPropertyRelative("tagName");

        var tagList = (from pTag in ProgressionManager.GetAllTags() select pTag.Name).ToList();

        var index = tagList.FindIndex(t => t.Contains(tagNameProperty.stringValue));

        var defaultGUIColor = GUI.color;

        if (index < 0)
            GUI.color = Color.yellow;

        EditorGUI.BeginChangeCheck();
        index = EditorGUI.Popup(position, index, tagList.ToArray());

        if (EditorGUI.EndChangeCheck())
        {
            tagNameProperty.stringValue = tagList[index];
        }

        EditorGUI.PropertyField(position, tagNameProperty, GUIContent.none);
        GUI.color = defaultGUIColor;

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}