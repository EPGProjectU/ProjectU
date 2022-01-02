using UnityEngine;

public partial class TagHook : ISerializationCallbackReceiver
{
    public string TagName
    {
        get => tagName;
        set
        {
            tagName = value;

            if (Application.isPlaying)
                ProgressionManager.Instance.ReRegisterRuntimeTagHook(this);
        }
    }

    [SerializeField]
    internal string tagName;

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

    internal void FireOnUpdate(TagEvent e)
    {
        OnUpdate.Invoke(e);
    }

    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
    }

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        ProgressionManager.RegisterTagHook(this);
    }
}