using System;
using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

// ReSharper disable InconsistentNaming

public class TagHookPlayModeFunctionality
{
    private ProgressionGraph _originalGraph;
    
    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // Swapping currently used graph for a new one for duration of test
        var data = Resources.Load<ProgressionManagerData>(ProgressionManager.DataPath);
        
        _originalGraph = data.graph;
        data.graph =  ScriptableObject.CreateInstance<ProgressionGraph>();
        
        // Populating the new graph
        for (var i = 0; i < 10; ++i)
        {
            var tagNode = data.graph.AddNode<TagNode>();
            tagNode.Name = "TestNode" + i;
        }
        
        // Init ProgressionManager using reflection
        typeof(ProgressionManager).GetMethod("Init", BindingFlags.Static | BindingFlags.NonPublic)?.Invoke(null, null);
        
        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        var data = Resources.Load<ProgressionManagerData>(ProgressionManager.DataPath);
        
        data.graph = _originalGraph;
        
        foreach (var gameObject in Object.FindObjectsOfType(typeof(MonoBehaviour)))
        {
            Object.Destroy(gameObject);
        }

        yield return null;
    }

    [UnityTest]
    public IEnumerator HookLinking()
    {
        var hook = TagHook.Create("TestNode0");
        var hookNonExisting = TagHook.Create("NonExisting");
        
        Assert.AreEqual(10, ProgressionManager.Data.graph.nodes.Count);
        Assert.AreSame(ProgressionManager.GetTag(hook.TagName), hook.Tag);
        Assert.NotNull(hook.Tag);
        Assert.AreSame(ProgressionManager.GetTag(hookNonExisting.TagName), hookNonExisting.Tag);
        ProgressionManager.Data.graph.AddNode<TagNode>();

        hook.Release();
        hookNonExisting.Release();
        yield return null;
    }

    private class StartUpInitializedHookClass : MonoBehaviour
    {
        private readonly TagHook _hookFieldInit = TagHook.Create("TestNode0");
        private TagHook _hookOnEnable;
        private TagHook _hookAwake;
        private TagHook _hookStart;

        private void OnEnable()
        {
            //_hookOnEnable.TagName = "TestNode0";
            _hookOnEnable = TagHook.Create("TestNode0");
        }

        private void Awake()
        {
            _hookAwake = TagHook.Create("TestNode0");
        }

        private void Start()
        {
            _hookStart = TagHook.Create("TestNode0");
        }

        private void OnDestroy()
        {
            _hookFieldInit.Release();
            _hookOnEnable.Release();
            _hookAwake.Release();
            _hookStart.Release();
        }

        public void CheckHookOnEnable()
        {
            Assert.AreEqual(ProgressionManager.GetTag(_hookOnEnable.TagName), _hookOnEnable.Tag);
        }
        
        public void CheckHookFieldInit()
        {
            Assert.AreEqual(ProgressionManager.GetTag(_hookFieldInit.TagName), _hookFieldInit.Tag);
        }

        public void CheckHookAwake()
        {
            Assert.AreEqual(ProgressionManager.GetTag(_hookAwake.TagName), _hookAwake.Tag);
        }

        public void CheckHookStart()
        {
            Assert.AreEqual(ProgressionManager.GetTag(_hookStart.TagName), _hookStart.Tag);
        }
    }
    
    [UnityTest]
    public IEnumerator StartUpInitializedHooks()
    {
        var hookClass = new GameObject().AddComponent<StartUpInitializedHookClass>();

        yield return null;

        hookClass.CheckHookOnEnable();
        hookClass.CheckHookAwake();
        hookClass.CheckHookStart();

        yield return null;
    }
}