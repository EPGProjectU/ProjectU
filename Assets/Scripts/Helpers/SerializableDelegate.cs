#nullable enable
using System;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

[Serializable]
public sealed class SerializableDelegate<TDelegate> : ISerializationCallbackReceiver where TDelegate : MulticastDelegate
{
    private TDelegate _delegate = null!;

    [SerializeField]
    private SerializableGUID guid;

    public static SerializableDelegate<TDelegate> operator +(SerializableDelegate<TDelegate> lhs, TDelegate rhs)
    {
        lhs._delegate = (Delegate.Combine(lhs._delegate, rhs) as TDelegate)!;

        return lhs;
    }

    public static SerializableDelegate<TDelegate> operator -(SerializableDelegate<TDelegate> lhs, TDelegate rhs)
    {
        lhs._delegate = (Delegate.Remove(lhs._delegate, rhs) as TDelegate)!;

        return lhs;
    }

    public void Invoke(params object?[]? e)
    {
        var invocationList = _delegate.GetInvocationList();

        foreach (var invocation in invocationList)
            invocation.DynamicInvoke(e);
    }

    public SerializableDelegate()
    {
        guid = SerializableGUID.Generate();
    }

    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
#if UNITY_EDITOR
        var invocationList = _delegate.GetInvocationList();

        var sb = new StringBuilder();

        foreach (var invocation in invocationList)
        {
            var info = invocation.GetMethodInfo();
            var gameObject = invocation.Target as UnityEngine.Object;

            if (gameObject == null)
                continue;

            sb.Append(gameObject.GetInstanceID());
            sb.Append('\t');
            sb.Append(gameObject.GetType().AssemblyQualifiedName);
            sb.Append('\t');
            sb.Append(info.Name);
            sb.Append('\n');
        }

        if (sb.Length > 0)
            sb.Length--;
        
        File.WriteAllText($"Temp/DelegateTempFile-{guid}", sb.ToString());
#endif
    }

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
#if UNITY_EDITOR
        // Delay call as Unity API is required
        EditorApplication.delayCall += () =>
        {
            if (!File.Exists($"Temp/DelegateTempFile-{guid}"))
                return;

            var serializationData = File.ReadAllText($"Temp/DelegateTempFile-{guid}");
            
            if (serializationData == "")
                return;

            foreach (var line in serializationData.Split('\n'))
            {
                var stringData = line.Split('\t');
                var instanceID = int.Parse(stringData[0]);
                var objectType = stringData[1];
                var methodInfo = stringData[2];

                // Get instance of object
                var obj = EditorUtility.InstanceIDToObject(instanceID);

                // Get class type
                var type = Type.GetType(objectType);

                // Get MethodInfo
                var info = type?.GetMethod(methodInfo, BindingFlags.Instance | BindingFlags.NonPublic);

                // Create delegate for the method
                var del = Delegate.CreateDelegate(
                    typeof(TDelegate),
                    obj,
                    info!, false);

                // Bind created delegate
                _delegate = (Delegate.Combine(_delegate, del) as TDelegate)!;
            }
        };
#endif
    }
}