using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using XNodeEditor;
using ProjectU.Core.Serialization;


[CustomNodeEditor(typeof(ParameterNode))]
public class ParameterNodeEditor : NodeEditor
{
    private ParameterNode _node;

    private ParameterNode Node => _node ??= target as ParameterNode;

    public override void OnBodyGUI()
    {
        serializedObject.Update();

        var port = Node.GetOutputPort(nameof(ParameterNode.data));
        port.ValueType = Node.ValueType;

        GUILayout.BeginHorizontal();
        Node.name = GUILayout.TextField(Node.name, GUILayout.Width(GetWidth() - 40f));
        NodeEditorGUILayout.PortField(new GUIContent(""), port, GUILayout.MinWidth(0));
        GUILayout.EndHorizontal();

        var typenameList = ParameterNode.Types.Select(type => ParameterNode.TypeNames.ContainsKey(type) ? ParameterNode.TypeNames[type] : type.PrettyName());
        var index = ParameterNode.Types.IndexOf(Node.ValueType);
        index = EditorGUILayout.Popup("", index, typenameList.ToArray());

        if (index > -1)
            Node.ValueType = ParameterNode.Types[index];

        serializedObject.ApplyModifiedProperties();
    }
}