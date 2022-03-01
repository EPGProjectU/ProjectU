using System.Collections;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TagHookEditorFunctionality
{
    private ProgressionGraph _originalGraph;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        ProgressionManager.ResetForTest();
        var data = Resources.Load<ProgressionManagerData>(ProgressionManager.DataPath);

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
        var data = Resources.Load<ProgressionManagerData>(ProgressionManager.DataPath);

        data.graph = _originalGraph;

        foreach (var gameObject in GameObject.FindObjectsOfType(typeof(MonoBehaviour)))
        {
            Object.DestroyImmediate(gameObject);
        }
        
        ProgressionManager.HardRefresh();

        yield return null;
    }

    [Test]
    public void HookRegistration()
    {
        var numberOfHooks = ProgressionManager.NumberOfRegisteredHooks();

        var hook = TagHook.Create("NonExisting");

        Assert.AreEqual(numberOfHooks + 1, ProgressionManager.NumberOfRegisteredHooks());
        Assert.AreEqual(null, hook.Tag);
    }

    [Test]
    public void TagRenamingOfRegisteredHook()
    {
        var numberOfHooks = ProgressionManager.NumberOfRegisteredHooks();


        var hook0 = TagHook.Create("TestNode0");
        var hookNonExisting = TagHook.Create("NonExisting");

        Assert.AreEqual("TestNode0", hook0.TagName);
        Assert.AreEqual("NonExisting", hookNonExisting.TagName);

        var nodes = ProgressionManager.Data.graph.nodes.OfType<TagNode>().ToList();

        foreach (var node in nodes)
        {
            node.Name += "_rename";
        }

        Assert.AreEqual("TestNode0_rename", hook0.TagName);
        Assert.AreEqual("NonExisting", hookNonExisting.TagName);
    }
}