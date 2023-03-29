using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

public class TagHookPlayModeFunctionality
{
    private ProgressionGraph _originalGraph;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        ProgressionManager.ClearCache();
        
        // Swapping currently used graph for a new one for duration of test
        var data = Resources.Load<ProgressionManagerData>(ProgressionManager.ResourceDataPath);

        _originalGraph = data.graph;
        data.graph = ScriptableObject.CreateInstance<ProgressionGraph>();

        // Populating the new graph
        for (var i = 0; i < 10; ++i)
        {
            var tagNode = data.graph.AddNode<TagNode>();
            tagNode.Name = "TestNode" + i;
        }

        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        foreach (var gameObject in Object.FindObjectsOfType(typeof(MonoBehaviour)))
            Object.Destroy(gameObject);

        ProgressionManager.Reload();

        yield return null;
    }

    [UnityTest]
    public IEnumerator HookLinking()
    {
        var hook = TagHook.Create("TestNode0");

        Assert.AreEqual(10, ProgressionManager.Data.graph.nodes.Count);
        Assert.AreSame(ProgressionManager.GetTag(hook.Tag.Name), hook.Tag);
        Assert.NotNull(hook.Tag);

        hook.Release();
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

        public void CheckHookFieldInit()
        {
            Assert.AreEqual(ProgressionManager.GetTag(_hookFieldInit.Tag.Name), _hookFieldInit.Tag);
        }

        public void CheckHookOnEnable()
        {
            Assert.AreEqual(ProgressionManager.GetTag(_hookOnEnable.Tag.Name), _hookOnEnable.Tag);
        }

        public void CheckHookAwake()
        {
            Assert.AreEqual(ProgressionManager.GetTag(_hookAwake.Tag.Name), _hookAwake.Tag);
        }

        public void CheckHookStart()
        {
            Assert.AreEqual(ProgressionManager.GetTag(_hookStart.Tag.Name), _hookStart.Tag);
        }
    }

    [UnityTest]
    public IEnumerator StartUpInitializedHooks()
    {
        var hookClass = new GameObject().AddComponent<StartUpInitializedHookClass>();

        yield return null;

        hookClass.CheckHookFieldInit();
        hookClass.CheckHookOnEnable();
        hookClass.CheckHookAwake();
        hookClass.CheckHookStart();

        yield return null;
    }
    
    [UnityTest]
    public IEnumerator HookUpdatePropagation()
    {
        var hook = TagHook.Create("TestNode0");

        var updateWasCalled = false;

        hook.onUpdate += update => updateWasCalled = true;

        ProgressionManager.SetActiveTag(hook.Tag, true);

        Assert.IsTrue(updateWasCalled);

        hook.Release();
        yield return null;
    }
}