using System;
using UnityEngine;

public partial class TagHook : ISerializationCallbackReceiver
{
    [SerializeField]
    internal string tagName;

    [NonSerialized]
    internal bool Registered;

    private SerializableGUID Guid = SerializableGUID.Generate();

    public SerializableGUID GetGUID() => Guid;

    public partial struct TagEvent
    {
        internal static TagEvent CreateInitEvent(TagHook hook)
        {
            return new TagEvent
            {
                Hook = hook,
                ProgressionTag = hook.Tag,
                OldState = hook.Tag.State,
                NewState = hook.Tag.State
            };
        }
    }

    public delegate void TagDelegate(TagEvent e);

    internal TagHook()
    {
    }

    private static TagHook CreateInternal(string tagName)
    {
        var hook = new TagHook
        {
            tagName = tagName
        };

        ProgressionManager.RegisterTagHook(hook);

        return hook;
    }

    private void TagNameSetInternal(string value)
    {
        tagName = value;

        if (Application.isPlaying)
            ProgressionManager.Instance.ReRegisterRuntimeTagHook(this);
    }

    internal void FireOnUpdate(TagEvent e)
    {
        OnUpdate?.Invoke(e);
    }

    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
    }

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        ProgressionManager.RegisterTagHook(this);
    }
}