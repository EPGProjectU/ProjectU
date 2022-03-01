using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ProgressionEditor : EditorWindow
{
    [MenuItem("ProjectU/Progression/Setting")]
    public static void ShowWindow()
    {
        GetWindow(typeof(ProgressionEditor));
    }

    void OnGUI()
    {
        GUILayout.Label("Main Graph", EditorStyles.boldLabel);

        if (ProgressionManager.Data == null)
            return;

        EditorGUI.BeginChangeCheck();
        var newGraph = EditorGUILayout.ObjectField(ProgressionManager.Data.graph, typeof(ProgressionGraph), false) as ProgressionGraph;

        if (!EditorGUI.EndChangeCheck())
            return;
        
        EditorUtility.SetDirty(ProgressionManager.Data);

        ProgressionManager.Data.graph = newGraph;
        ProgressionManager.HardRefresh();
    }
}