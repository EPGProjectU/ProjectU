using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PathPatrolDefinition))]
public class PathPatrolDefinitionEditior : Editor
{
    private void OnSceneGUI()
    {
        EditorGUI.BeginChangeCheck();
        var points = serializedObject.FindProperty(nameof(PathPatrolDefinition.path));

        if (points.arraySize == 0)
            return;
        
        var firstPoint = points.GetArrayElementAtIndex(0).vector3Value;
        var lastPoint = firstPoint;
        
        
        for(var i = 0; i < points.arraySize; i++)
        {
            var point = points.GetArrayElementAtIndex(i).vector3Value;
            points.GetArrayElementAtIndex(i).vector3Value = Handles.FreeMoveHandle(point, Quaternion.identity, 0.2f, Vector3.zero, Handles.SphereHandleCap);
            
            Handles.DrawLine(point, lastPoint, 3.0f);
            
            lastPoint = point;
        }
        
        if (serializedObject.FindProperty(nameof(PathPatrolDefinition.loopPath)).boolValue)
            Handles.DrawLine(lastPoint, firstPoint, 3.0f);
        
        if (EditorGUI.EndChangeCheck())
            serializedObject.ApplyModifiedProperties();
    }
}
