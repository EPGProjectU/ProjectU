using UnityEditor;
using UnityEngine;

/// <summary>
/// Window for controlling settings of progression system
/// </summary>
public class ProgressionSettingsEditor : EditorWindow
{
    [MenuItem("ProjectU/Progression/Setting")]
    public static void ShowWindow()
    {
        GetWindow(typeof(ProgressionSettingsEditor));
    }

    private void OnGUI()
    {
        EditorGUI.BeginChangeCheck();
        GUILayout.Label("Main Graph", EditorStyles.boldLabel);
        var newGraph = EditorGUILayout.ObjectField(ProgressionManager.Data.graph, typeof(ProgressionGraph), false) as ProgressionGraph;

        if (!EditorGUI.EndChangeCheck())
            return;

        // Makes unity save change to gameobject storing data
        EditorUtility.SetDirty(ProgressionManager.Data);

        ProgressionManager.Data.graph = newGraph;
        ProgressionManager.Reload();
    }
}