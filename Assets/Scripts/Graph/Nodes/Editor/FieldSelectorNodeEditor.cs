using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using XNodeEditor;


[CustomNodeEditor(typeof(FieldSelectorNode))]
public class FieldSelectorNodeEditor : NodeEditor
{
    private FieldSelectorNode _node;
    private FieldSelectorNode Node => _node ??= target as FieldSelectorNode;

    public override void OnBodyGUI()
    {
        serializedObject.Update();

        var inputPort = Node.GetInputPort(nameof(FieldSelectorNode.target));
        var outputPort = Node.GetOutputPort(nameof(FieldSelectorNode.value));


        GUILayout.BeginHorizontal();
        NodeEditorGUILayout.PortField(GUIContent.none, inputPort, GUILayout.MinWidth(0));

        inputPort.ValueType = typeof(object);
        outputPort.ValueType = typeof(object);

        if (Node.HasRoot() && inputPort.GetConnection(0) is { node: ITypeNode { ValueType: {} } parameterNode })
        {
            var fields = parameterNode.ValueType.GetFields();

            var properties = parameterNode.ValueType.GetProperties();

            if (fields.Length > 0)
            {
                var fieldNames = from field in fields
                    select $"{field.FieldType.PrettyName()} {field.Name}";

                // try to find exact match
                var index = Array.IndexOf(fields, Node.selectedField);

                if (index < 0)
                {
                    var closestMatch = fields.FirstOrDefault(info => info == Node.selectedField);

                    if (closestMatch == null)
                        closestMatch = fields.FirstOrDefault(info => info.Name == Node.selectedField?.Name);

                    if (closestMatch == null)
                        closestMatch = fields.FirstOrDefault(info => info.FieldType == Node.selectedField?.FieldType);

                    index = closestMatch != null ? Array.IndexOf(fields, closestMatch) : 0;
                }

                index = EditorGUILayout.Popup("", index, fieldNames.ToArray());

                Node.selectedField = fields[index];

                inputPort.ValueType = parameterNode.ValueType;
                outputPort.ValueType = Node.selectedField.FieldType;
            }
        }

        NodeEditorGUILayout.PortField(GUIContent.none, outputPort, GUILayout.MinWidth(0));
        GUILayout.EndHorizontal();


        serializedObject.ApplyModifiedProperties();
    }
}