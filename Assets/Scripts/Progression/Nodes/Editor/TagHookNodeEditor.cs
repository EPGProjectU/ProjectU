using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

/// <summary>
/// Draws <see cref="TagNode"/> in <see cref="NodeGraphEditor"/>
/// </summary>
[CustomNodeEditor(typeof(TagHookNode))]
public class TagHookEditor : NodeEditor
{
    private TagHookNode _node;
    private TagHookNode Node => _node ??= target as TagHookNode;

    public override void OnBodyGUI()
    {
        serializedObject.Update();

        ProgressionManager.StartEditorChange();

        var inputStates = new[]
        {
            TagNode.TagState.Unavailable,
            TagNode.TagState.Available,
            TagNode.TagState.Active,
            TagNode.TagState.Collected
        };


        var outputStates = new[]
        {
            TagNode.TagState.Active,
            TagNode.TagState.Collected
        };

        var input = target.GetInputPort("input");
        var output = target.GetOutputPort("output");

        input.ValueType = typeof(NotifyNodeInterface.EmptyPort);


        var inputStateIndex = Array.FindIndex(inputStates, state => state == _node.input);
        var outputStateIndex = Array.FindIndex(outputStates, state => state == _node.output);

        EditorGUI.BeginChangeCheck();

        var hookProperty = serializedObject.FindProperty(nameof(TagHookNode.tagHook));
        EditorGUILayout.PropertyField(hookProperty, GUIContent.none);

        GUILayout.BeginHorizontal();

        switch (output.IsConnected)
        {
            case true:
                inputStateIndex = EditorGUILayout.Popup(inputStateIndex, inputStates.Select(state => state.ToString()).ToArray());
                break;
            case false:
                NodeEditorGUILayout.PortField(new GUIContent("In"), input, GUILayout.MinWidth(0));
                break;
        }


        switch (input.IsConnected)
        {
            case true:
                outputStateIndex = EditorGUILayout.Popup(outputStateIndex, outputStates.Select(state => state.ToString()).ToArray());
                break;
            case false:
                NodeEditorGUILayout.PortField(new GUIContent("Out"), output, GUILayout.MinWidth(0));
                break;
        }

        GUILayout.EndHorizontal();


        if (EditorGUI.EndChangeCheck())
        {
            if (inputStateIndex >= 0)
                _node.input = inputStates[inputStateIndex];

            if (outputStateIndex >= 0)
                _node.output = outputStates[outputStateIndex];
            //EditorUtility.SetDirty(_node);
        }

        serializedObject.ApplyModifiedProperties();
    }

    public override int GetWidth() => 160;

    public override Color GetTint()
    {
        return Node.tagHook.Tag?.State switch
        {
            TagNode.TagState.Available => new Color(0.27f, 0.39f, 0.28f),
            TagNode.TagState.Collected => new Color(0.13f, 0.2f, 0.14f),
            TagNode.TagState.Active => new Color(0.13f, 0.22f, 0.33f),
            _ => base.GetTint()
        };
    }
}