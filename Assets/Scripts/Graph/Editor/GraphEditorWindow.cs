using System.Linq;
using UnityEngine;
using XNode;
using XNodeEditor;

/// <summary>
/// <see cref="QuestGraph"/> editor window
/// </summary>
public class GraphEditorWindow : NodeEditorWindow
{
    public static T Open<T>(NodeGraph graph) where T : GraphEditorWindow
    {
        if (!graph) return null;

        var windows = Resources.FindObjectsOfTypeAll<T>();

        var window = windows.FirstOrDefault(window => window.graph == graph);

        if (window == null)
            window = CreateInstance<T>();

        window.graph = graph;

        
        window.Show();

        return window;
    }
}