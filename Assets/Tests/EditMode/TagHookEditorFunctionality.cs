using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.TestTools;

public class TagHookEditorFunctionality
{
    private ProgressionGraph _originalGraph;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // Reset ProgressionManager using reflection
        typeof(ProgressionManager).GetMethod("Reset", BindingFlags.Static | BindingFlags.NonPublic)?.Invoke(null, null);

        var data = Resources.Load<ProgressionManagerData>(ProgressionManager.ResourceDataPath);

        _originalGraph = data.graph;
        data.graph = ScriptableObject.CreateInstance<ProgressionGraph>();

        for (var i = 0; i < 10; ++i)
        {
            var tagNode = data.graph.AddNode<TagNode>();
            tagNode.Name = "TestNode" + i;
        }

        ProgressionManager.Reload();

        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        var data = Resources.Load<ProgressionManagerData>(ProgressionManager.ResourceDataPath);

        data.graph = _originalGraph;

        foreach (var gameObject in Object.FindObjectsOfType(typeof(MonoBehaviour)))
            Object.DestroyImmediate(gameObject);

        ProgressionManager.Reload();

        yield return null;
    }
}