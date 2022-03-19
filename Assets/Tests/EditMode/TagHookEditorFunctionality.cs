using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
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

        ProgressionManager.HardRefresh();

        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        var data = Resources.Load<ProgressionManagerData>(ProgressionManager.ResourceDataPath);

        data.graph = _originalGraph;

        foreach (var gameObject in Object.FindObjectsOfType(typeof(MonoBehaviour)))
            Object.DestroyImmediate(gameObject);

        ProgressionManager.HardRefresh();

        yield return null;
    }

    private static int NumberOfRegisteredHooks()
    {
        if (typeof(ProgressionManager).GetField("HookRegistry", BindingFlags.Static | BindingFlags.NonPublic)?.GetValue(null) is List<TagHook> registry)
            return registry.Count;

        return -1;
    }

    [Test]
    public void HookRegistration()
    {
        var numberOfHooks = NumberOfRegisteredHooks();

        var hook = TagHook.Create("NonExisting");

        Assert.AreEqual(numberOfHooks + 1, NumberOfRegisteredHooks());
        Assert.AreEqual(null, hook.Tag);
    }

    [Test]
    public void TagRenamingOfRegisteredHook()
    {
        var hook0 = TagHook.Create("TestNode0");
        var hookNonExisting = TagHook.Create("NonExisting");

        Assert.AreEqual("TestNode0", hook0.TagName);
        Assert.AreEqual("NonExisting", hookNonExisting.TagName);

        var nodes = ProgressionManager.Data.graph.nodes.OfType<TagNode>().ToList();

        foreach (var node in nodes)
            node.Name += "_rename";

        Assert.AreEqual("TestNode0_rename", hook0.TagName);
        Assert.AreEqual("NonExisting", hookNonExisting.TagName);
    }
}