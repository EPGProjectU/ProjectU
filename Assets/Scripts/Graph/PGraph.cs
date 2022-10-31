using System;
using System.Collections.Generic;
using System.Linq;
using ProjectU.Core.Helpers;
using ProjectU.Core.Serialization;
using UnityEngine;

public class Graph : XNode.NodeGraph
{
    public SerializedDictionary<ParameterNode, object> Context { get; internal set; }
}

[Serializable]
public class PGraph<TGraph> : ISerializationCallbackReceiver where TGraph : Graph
{
    [SerializeField]
    private TGraph graph;

    [SerializeField]
    private SerializedDictionary<ParameterNode, object> parameters = new SerializedDictionary<ParameterNode, object>();


    public TGraph Graph
    {
        get
        {
            if (graph != null)
                graph.Context = Parameters;

            return graph;
        }
        set
        {
            graph = value;
            ConvertParameters();
        }
    }

    public SerializedDictionary<ParameterNode, object> Parameters
    {
        get => parameters;
        private set => parameters = value;
    }

    public Type GetGraphType()
    {
        return typeof(TGraph);
    }

    /// <summary>
    /// QoL functionality that attempts to convert current values of parameters to the ones in the new graph
    /// </summary>
    /// <remarks>
    /// For it to work parameters have to have exactly the same name and current value must be assignable to the new parameter
    /// </remarks>
    private void ConvertParameters()
    {
        if (graph == null)
        {
            parameters.Clear();
            return;
        }

        var parameterNodes = graph.nodes.OfType<ParameterNode>().ToArray();

        var newParameters = new SerializedDictionary<ParameterNode, object>();

        foreach (var parameterNode in parameterNodes)
        {
            var match = parameters.Keys
                .FirstOrDefault(key => key.name.Equals(parameterNode.name)
                                       && parameterNode.ValueType.IsAssignableFrom(key.ValueType));

            newParameters[parameterNode] = match != null ? parameters[match] : parameterNode.ValueType.CreateDefaultValue();
        }

        parameters = newParameters;
    }

    /// <summary>
    /// Ensures that all and only parameters present in graph are stored and have correct values
    /// </summary>
    public void ValidateParameters()
    {
        var parameterNodes = graph.nodes.OfType<ParameterNode>().ToArray();

        // Filter out parameters not present in the graph
        // ToList() is called to sever ties with the parameters enumerator which cannot be used due calling Remove on it
        foreach (var kvp in parameters.Where(kvp => !parameterNodes.Contains(kvp.Key)).ToList())
        {
            parameters.Remove(kvp.Key);
        }

        // Add missing parameters
        foreach (var parameterNode in parameterNodes)
        {
            parameters.TryGetValue(parameterNode, out var value);

            value = parameterNode.ValueType.SetOrDefaultValue(value);

            parameters[parameterNode] = value;
        }
    }

    void ISerializationCallbackReceiver.OnBeforeSerialize() {}

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        if (graph != null)
            ValidateParameters();
    }
}