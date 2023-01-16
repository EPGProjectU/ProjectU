using System;
using System.Collections.Generic;
using System.Linq;
using ProjectU.Core.Helpers;
using ProjectU.Core.Serialization;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;


[CustomPropertyDrawer(typeof(PGraph<>), true)]
public class ParameterizedGraphPropertyDrawer : PropertyDrawer
{
    public static readonly Dictionary<Type, Func<ParameterNode, object, Rect, object>> ParameterFieldSelector = new Dictionary<Type, Func<ParameterNode, object, Rect, object>>();

    private const float FIELD_MARGIN = 2f;
    private const float FIELD_INDENT = 10f;

    private static bool Foldout
    {
        get => SessionState.GetBool("AIControllerEditorFoldout", false);
        set => SessionState.SetBool("AIControllerEditorFoldout", value);
    }

    static ParameterizedGraphPropertyDrawer()
    {
        ParameterFieldSelector[typeof(string)] = (parameter, initialValue, pos) => EditorGUI.TextField(pos, parameter.name, (string)initialValue);
        ParameterFieldSelector[typeof(bool)] = (parameter, initialValue, pos) => EditorGUI.Toggle(pos, parameter.name, (bool)initialValue);
        ParameterFieldSelector[typeof(int)] = (parameter, initialValue, pos) => EditorGUI.IntField(pos, parameter.name, (int)initialValue);
        ParameterFieldSelector[typeof(long)] = (parameter, initialValue, pos) => EditorGUI.LongField(pos, parameter.name, (long)initialValue);
        ParameterFieldSelector[typeof(float)] = (parameter, initialValue, pos) => EditorGUI.FloatField(pos, parameter.name, (float)initialValue);
        ParameterFieldSelector[typeof(double)] = (parameter, initialValue, pos) => EditorGUI.DoubleField(pos, parameter.name, (double)initialValue);
        ParameterFieldSelector[typeof(object)] = (parameter, initialValue, pos) => EditorGUI.ObjectField(pos, parameter.name, (Object)initialValue, parameter.ValueType, true);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        position.height = EditorGUIUtility.singleLineHeight;
        var change = false;

        var isSceneObject = (property.serializedObject.targetObject as MonoBehaviour)?.gameObject.scene.rootCount > 0;

        var graphProperty = property.FindPropertyRelative("graph");
        var graph = graphProperty.objectReferenceValue as Graph;

        dynamic parameterizedGraph = property.serializedObject.targetObject.GetReflectionFieldValue(property.name);

        var graphType = parameterizedGraph.GetGraphType() as Type;

        EditorGUI.BeginChangeCheck();
        var newGraph = EditorGUI.ObjectField(position, property.displayName, graph, graphType, false);
        change |= EditorGUI.EndChangeCheck();

        if (graph != newGraph)
        {
            graph = newGraph as Graph;
            parameterizedGraph.Graph = Convert.ChangeType(graph, parameterizedGraph.GetGraphType());
        }

        if (graph != null && parameterizedGraph.Parameters is SerializedDictionary<ParameterNode, object> parameters)
        {
            parameterizedGraph.ValidateParameters();

            var parameterNodes = graph.nodes.OfType<ParameterNode>().ToList();

            if (parameterNodes.Count > 0)
            {
                Foldout = EditorGUI.Foldout(position, Foldout, GUIContent.none);
            }

            parameterNodes.Sort((nodeA, nodeB) => (int)nodeA.position.y - (int)nodeB.position.y);

            if (Foldout)
            {
                EditorGUI.indentLevel++;
                position.x += FIELD_INDENT;
                position.width -= FIELD_INDENT;

                foreach (var node in parameterNodes)
                {
                    position.y += position.height + FIELD_MARGIN;

                    var type = node.ValueType;

                    EditorGUI.BeginDisabledGroup(type.IsSubclassOf(typeof(MonoBehaviour)) && !isSceneObject);

                    if (!ParameterFieldSelector.ContainsKey(type))
                        type = typeof(object);

                    var currentValue = parameters[node];

                    EditorGUI.BeginChangeCheck();
                    var newValue = ParameterFieldSelector[type].Invoke(node, currentValue, position);
                    change |= EditorGUI.EndChangeCheck();


                    if (!Equals(currentValue, newValue))
                    {
                        Undo.RecordObject(property.serializedObject.targetObject, "Parameter Container edited");
                        parameters[node] = newValue;
                    }

                    EditorGUI.EndDisabledGroup();
                }

                EditorGUI.indentLevel--;
            }
        }

        if (change)
        {
            EditorUtility.SetDirty(property.serializedObject.targetObject);
            property.serializedObject.ApplyModifiedProperties();
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        dynamic parameterizedGraph = property.serializedObject.targetObject.GetReflectionFieldValue(property.name);

        if (parameterizedGraph.Graph == null)
            return base.GetPropertyHeight(property, label);

        parameterizedGraph.ValidateParameters();

        var numberOfLines = Foldout ? parameterizedGraph.Parameters.Count + 1 : 1;
        return numberOfLines * (base.GetPropertyHeight(property, label) + FIELD_MARGIN) - FIELD_MARGIN;
    }
}