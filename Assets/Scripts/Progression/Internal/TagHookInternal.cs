using System;
using UnityEngine;

// Internal functionality of TagHook
[Serializable]
public partial class TagHook
{
    /// <summary>
    /// Delegate used in <see cref="TagHook.onUpdate"/>
    /// </summary>
    public delegate void UpdateDelegateWithEvent(TagEvent e);

    public delegate void UpdateDelegate();

    internal TagHook()
    {
        onUpdate = new HookUpdateDelegate<UpdateDelegateWithEvent>(this);
        onCollect = new HookUpdateDelegate<UpdateDelegate>(this);
        onActivate = new HookUpdateDelegate<UpdateDelegate>(this);
    }

    /// <summary>
    /// Implementation of <see cref="Create"/>
    /// </summary>
    /// <param name="tagName"></param>
    /// <returns>Instantiated <see cref="TagHook"/></returns>
    private static TagHook Create_Impl(string tagName)
    {
        var hook = new TagHook();

        hook.Tag = ProgressionManager.GetTag(tagName);
        //ProgressionManager.OnInitialization += () =>
        //{
        //    hook.Tag = ProgressionManager.GetTag(tagName);
        //    hook.Bind();
        //};

        hook.Bind();

        return hook;
    }

    /// <summary>
    /// Implementation of <see cref="Release"/>
    /// </summary>
    private void Release_Impl()
    {
        Unbind();
        onUpdate.Reset();
        onCollect.Reset();
        onActivate.Reset();
    }

    /// <summary>
    /// Fires <see cref="onUpdate"/>
    /// </summary>
    /// <param name="e"><see cref="TagEvent"/></param>
    internal void FireOnUpdate(TagEvent e)
    {
        onUpdate.Invoke(e);
        onCollect.Invoke(e);
        onActivate.Invoke(e);
    }

    internal void Bind()
    {
        if (Tag == null)
            return;

        ProgressionManager.BindUpdate(Tag, FireOnUpdate);
    }

    internal void Unbind()
    {
        if (Tag == null)
            return;

        ProgressionManager.UnbindUpdate(Tag, FireOnUpdate);
    }

    [Serializable]
    public class HookUpdateDelegate<TDelegate> : ISerializationCallbackReceiver
        where TDelegate : MulticastDelegate
    {
        [SerializeField]
        private SerializedDelegate<TDelegate> @delegate;

        [NonSerialized]
        private TagHook _hook;

        internal HookUpdateDelegate(TagHook tagHook)
        {
            _hook = tagHook;
        }

        internal void Invoke(params object?[]? e)
        {
            @delegate?.Invoke(e);
        }

        internal void Reset()
        {
            @delegate = null;
        }

        public static HookUpdateDelegate<TDelegate> operator +(HookUpdateDelegate<TDelegate> lhs, TDelegate rhs)
        {
            lhs.@delegate += rhs;
            lhs._hook.Bind();

            return lhs;
        }

        public static HookUpdateDelegate<TDelegate> operator -(HookUpdateDelegate<TDelegate> lhs, TDelegate rhs)
        {
            lhs.@delegate -= rhs;

            return lhs;
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize() {}

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if (@delegate != null)
                _hook.Bind();
        }
    }
}