using System;
using UnityEngine;

// Internal functionality of TagHook
[Serializable]
public partial class TagHook: ISerializationCallbackReceiver
{
    [SerializeField]
    internal string tagName;

    /// <summary>
    /// Delegate used in <see cref="TagHook.onUpdate"/>
    /// </summary>
    public delegate void TagDelegate(TagEvent e);

    internal TagHook() {}

    /// <summary>
    /// Implementation of <see cref="Create"/>
    /// </summary>
    /// <param name="tagName"></param>
    /// <returns>Instantiated <see cref="TagHook"/></returns>
    private static TagHook Create_Impl(string tagName)
    {
        var hook = new TagHook();

        ProgressionManager.RegisterTagHook(hook);

        hook.SetTagName_Impl(tagName);

        return hook;
    }

    /// <summary>
    /// Implementation of <see cref="Release"/>
    /// </summary>
    private void Release_Impl()
    {
        ProgressionManager.UnRegisterTagHook(this);
        onUpdate = null;
    }

    /// <summary>
    /// Implementation of <see cref="set_TagName"/>
    /// </summary>
    /// <param name="value">New <see cref="TagName"/></param>
    private void SetTagName_Impl(string value)
    {
        if (tagName == value)
            return;

        tagName = value;

        // ReLinks hook under the new name
        ProgressionManager.ReLinkTagHook(this);
    }

    /// <summary>
    /// Implementation of <see cref="GetDummyTagEvent"/>
    /// </summary>
    /// <returns></returns>
    private TagEvent GetDummyTagEvent_Impl()
    {
        var tagEvent = new TagEvent
        {
            hook = this,
            progressionTag = Tag
        };

        if (Tag == null)
            return tagEvent;

        tagEvent.oldState = Tag.State;
        tagEvent.newState = Tag.State;

        return tagEvent;
    }

    /// <summary>
    /// Fires <see cref="onUpdate"/>
    /// </summary>
    /// <param name="e"><see cref="TagEvent"/></param>
    internal void FireOnUpdate(TagEvent e)
    {
        onUpdate?.Invoke(e);
    }

    void ISerializationCallbackReceiver.OnBeforeSerialize() {}

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        ProgressionManager.RegisterTagHook(this);
    }
}