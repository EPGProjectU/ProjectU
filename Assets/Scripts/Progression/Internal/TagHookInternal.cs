using System;
using UnityEngine;

[Serializable]
public partial class TagHook : ISerializationCallbackReceiver
{
    [SerializeField]
    internal string tagName;

    public delegate void TagDelegate(TagEvent e);
    
    [SerializeField]
    private SerializableGUID guid = SerializableGUID.Generate(); 

    internal TagHook()
    {
    }

    private static TagHook Create_Impl(string tagName)
    {
        var hook = new TagHook();

        ProgressionManager.RegisterTagHook(hook);

        hook.SetTagName_Impl(tagName);
        
        return hook;
    }
    
    private void Release_Impl()
    {
        ProgressionManager.UnRegisterTagHook(this);
        onUpdate = null;
        onInitialization = null;
    }

    private void SetTagName_Impl(string value)
    {
        if (tagName == value)
            return;
        
        tagName = value;

        ProgressionManager.ReLinkTagHook(this);
    }

    internal void FireOnUpdate(TagEvent e)
    {
        onUpdate?.Invoke(e);
    }

    internal void FireOnInitialization()
    {
        onInitialization?.Invoke(new TagEvent()
        {
            hook = this,
            progressionTag = Tag,
            oldState = Tag.State,
            newState = Tag.State
        });
    }

    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
    }

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        ProgressionManager.RegisterTagHook(this);
    }
}