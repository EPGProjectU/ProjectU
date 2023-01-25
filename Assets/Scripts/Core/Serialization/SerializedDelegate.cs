#nullable enable
using System;
using ProjectU.Core.Serialization;
using UnityEngine;

/// <summary>
/// Delegate that persist through a HotReload
/// </summary>
[Serializable]
public sealed class SerializedDelegate<TDelegate> : ISerializationCallbackReceiver where TDelegate : MulticastDelegate
{
    private TDelegate? _delegate;

    private SerializationBundle _bundle = new SerializationBundle();

    public static SerializedDelegate<TDelegate> operator +(SerializedDelegate<TDelegate>? lhs, TDelegate rhs)
    {
        lhs ??= new SerializedDelegate<TDelegate>();

        lhs._delegate = (Delegate.Combine(lhs._delegate, rhs) as TDelegate)!;

        return lhs;
    }

    public static SerializedDelegate<TDelegate> operator -(SerializedDelegate<TDelegate>? lhs, TDelegate rhs)
    {
        if (lhs == null)
            return new SerializedDelegate<TDelegate>();

        lhs._delegate = (Delegate.Remove(lhs._delegate, rhs) as TDelegate)!;

        return lhs;
    }

    public void Invoke(params object?[]? e) => _delegate?.DynamicInvoke(e);

    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
        _bundle.Serialize(_delegate);
    }

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        _delegate = _bundle.Deserialize() as TDelegate;
    }
}