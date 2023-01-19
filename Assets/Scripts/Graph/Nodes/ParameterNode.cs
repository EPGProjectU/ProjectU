using System;
using System.Collections.Generic;
using ProjectU.Core.Helpers;
using ProjectU.Core.Serialization;
using UnityEngine;
using XNode;


[CreateNodeMenu("Data/Parameter", 0)]
public class ParameterNode : Node, ITypeNode
{
    public new string name = "";

    [SerializeField]
    private SerializedType serializableType = Types[0];

    public Type ValueType
    {
        get => serializableType.Type;
        set => serializableType.Type = value;
    }

    public bool HasRoot() => true;

    [Output]
    public object data;

    // Return the correct value of an output port when requested
    public override object GetValue(NodePort port)
    {
        if (graph is Graph { Context: {} } gr)
        {
            gr.Context.TryGetValue(this, out var value);
            return value;
        }

        return null;
    }

    public void SetValue(object value)
    {
        if (graph is Graph { Context: {} } gr)
        {
            gr.Context[this] = value;
        }
    }

    public static readonly Dictionary<Type, string> TypeNames = new Dictionary<Type, string>();

    public static readonly List<Type> Types = new List<Type>
    {
        typeof(string),
        typeof(bool),
        typeof(int),
        typeof(long),
        typeof(float),
        typeof(double),
        // Additional types can be added by adding [ParameterType] attribute to class definition or by calling AddType.
        // In case of calling AddType it is important to note that given type will not appear as a dropdown selection until code calling the method is executed
    };

    public static void AddType(Type type, string friendlyName)
    {
        if (Types.Contains(type))
            return;

        Types.Add(type);
        TypeNames.Add(type, friendlyName);
    }

    static ParameterNode()
    {
        foreach (var type in typeof(ParameterTypeAttribute).GetTypesWithAttribute())
        {
            var attribute = (ParameterTypeAttribute)Attribute.GetCustomAttribute(type, typeof(ParameterTypeAttribute));

            AddType(type, attribute.FriendlyName ?? type.Name);
        }
    }
}

/// <summary>
/// Adds class to ParameterNode's types
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class ParameterTypeAttribute : Attribute
{
    public string FriendlyName;
}