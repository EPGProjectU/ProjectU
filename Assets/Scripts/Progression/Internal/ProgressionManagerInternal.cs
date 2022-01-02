using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using XNode;

public partial class ProgressionManager
{
    private Dictionary<string, ProgressionTag> _tags = new Dictionary<string, ProgressionTag>();

    private static readonly List<WeakReference<TagHook>> HookRegistry = new List<WeakReference<TagHook>>();

    private Dictionary<ProgressionTag, List<WeakReference<TagHook>>> _hookCallList = new Dictionary<ProgressionTag, List<WeakReference<TagHook>>>();

    private List<TagEventBuilder> _tagEventBuilders;

    private bool _dirty = true;


    private static ProgressionManager _instance;

    public static ProgressionManager Instance
    {
        get
        {
            if (_instance != null)
                return _instance;

            var pm = FindObjectsOfType<ProgressionManager>();

            if (pm.Length > 0)
                return _instance = pm[0];

            // Creating new instance if does not exist
            return _instance = new GameObject("Progression Manager").AddComponent<ProgressionManager>();
        }
    }

    private void Start()
    {
        if (progressionGraph != null)
            progressionGraph = progressionGraph.Copy() as ProgressionGraph;

        InitTagEventBuilders();
        ReinitializeTagReferences();

        LinkHooks();

        foreach (var hookReference in HookRegistry)
            if (hookReference.TryGetTarget(out var hook) && hook.Tag != null)
                hook.OnInitialization?.Invoke(TagHook.TagEvent.CreateInitEvent(hook));

        InitTagEventBuilders();
    }

    private void Update()
    {
        if (!_dirty)
            return;

        RemoveDeadHookReferences();
        _dirty = false;
    }

    internal static void RegisterTagHook(TagHook hook)
    {
        HookRegistry.Add(new WeakReference<TagHook>(hook));
    }

    internal void RegisterRuntimeTagHook(TagHook hook)
    {
        var hookReference = new WeakReference<TagHook>(hook);
        HookRegistry.Add(hookReference);

        hook.Tag = _tags[hook.TagName];
        _hookCallList[hook.Tag].Add(hookReference);
        hook.OnInitialization?.Invoke(TagHook.TagEvent.CreateInitEvent(hook));
    }

    internal void ReRegisterRuntimeTagHook(TagHook hook)
    {
        if (hook.Tag == null)
            return;

        var hookReference = new WeakReference<TagHook>(hook);

        // Removing hook under the tag reference from the call list
        _hookCallList[hook.Tag].RemoveAll(reference => !reference.TryGetTarget(out var oldHook) || oldHook == hook);

        hook.Tag = _tags[hook.TagName];
        _hookCallList[hook.Tag].Add(hookReference);
    }

    [Serializable]
    public class TagEventBuilder
    {
        public ProgressionTag.TagState OldState;
        public ProgressionTag.TagState NewState;
        public ProgressionTag ProgressionTag;

        public TagHook.TagEvent GetEvent(TagHook hook)
        {
            return new TagHook.TagEvent
            {
                Hook = hook,
                ProgressionTag = ProgressionTag,
                OldState = OldState,
                NewState = NewState
            };
        }
    }

    private void StartChange()
    {
        UpdateTagEventBuilders();
    }

    private void EndChange()
    {
        UpdateTagEventBuilders();
        SendTagUpdateEvents();
    }

#if UNITY_EDITOR
    public static void StartEditorChange()
    {
        if (!Application.isPlaying)
            return;

        Instance.UpdateTagEventBuilders();
    }

    public static void EndEditorChange(bool fireEvents = true)
    {
        if (!Application.isPlaying)
            return;

        Instance.UpdateTagEventBuilders();

        if (fireEvents)
            Instance.SendTagUpdateEvents();
    }
#endif

    /// <summary>
    /// Sends tag update event to all hook that linked to tags that had changed
    /// </summary>
    private void SendTagUpdateEvents()
    {
        foreach (var tagEventBuilder in _tagEventBuilders)
        {
            if (tagEventBuilder.NewState == tagEventBuilder.OldState)
                continue;

            foreach (var hookReference in _hookCallList[tagEventBuilder.ProgressionTag])
            {
                if (!hookReference.TryGetTarget(out var hook))
                {
                    _dirty = true;

                    continue;
                }

                hook.FireOnUpdate(tagEventBuilder.GetEvent(hook));
            }
        }
    }

    private void RemoveDeadHookReferences()
    {
        HookRegistry.RemoveAll(reference => !reference.TryGetTarget(out _));

        foreach (var hookReference in _hookCallList)
            hookReference.Value.RemoveAll(reference => !reference.TryGetTarget(out _));
    }

    private void ReinitializeTagReferences()
    {
        _tags.Clear();

        if (progressionGraph == null)
            return;

        var nodeList = progressionGraph.nodes.OfType<ProgressionTag>().ToList();

        nodeList.Sort((left, right) => string.CompareOrdinal(left.Name, right.Name));

        foreach (var progressionTag in nodeList)
            _tags.Add(progressionTag.Name, progressionTag);
    }

    private void LinkHooks()
    {
        _hookCallList.Clear();

        foreach (var pair in _tags)
            _hookCallList[pair.Value] = new List<WeakReference<TagHook>>();

        RemoveDeadHookReferences();

        foreach (var hookReference in HookRegistry)
        {
            if (!hookReference.TryGetTarget(out var hook))
                continue;

            if (hook.TagName == "")
            {
                Debug.LogWarning("Hook is missing tag name!");

                continue;
            }

            if (!_tags.ContainsKey(hook.TagName))
            {
                Debug.LogWarning($"Could not link hook for {hook.TagName}! Tag {hook.TagName} does not exist in the current context!");

                continue;
            }

            hook.Tag = _tags[hook.TagName];
            _hookCallList[hook.Tag].Add(hookReference);
        }
    }
    private void InitTagEventBuilders()
    {
        _tagEventBuilders = _tags.Select(progressionTag => new TagEventBuilder { NewState = progressionTag.Value.State, ProgressionTag = progressionTag.Value }).ToList();
    }

    private void UpdateTagEventBuilders()
    {
        foreach (var tagEventBuilder in _tagEventBuilders)
        {
            tagEventBuilder.OldState = tagEventBuilder.NewState;
            tagEventBuilder.NewState = tagEventBuilder.ProgressionTag.State;
        }
    }
    public static void SendTagNameChangeNotifications(string oldName, string newName, NodeGraph nodeGraph)
    {
        if (nodeGraph != Instance.progressionGraph)
            return;

        foreach (var hookReference in HookRegistry)
        {
            if (!hookReference.TryGetTarget(out var hook))
                continue;

            if (hook.TagName == oldName)
                hook.tagName = newName;
        }
    }

#if UNITY_EDITOR
    public void OnValidate()
    {
        RemoveDeadHookReferences();

        if (Application.isPlaying)
        {
            ReinitializeTagReferences();
            LinkHooks();
        }
        else
        {
            EditorApplication.delayCall += ReinitializeTagReferences;
        }

        InitTagEventBuilders();
    }
#endif
}
