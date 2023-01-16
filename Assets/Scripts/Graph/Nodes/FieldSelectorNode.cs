using System;
using System.Reflection;
using XNode;

[CreateNodeMenu("Data/FieldSelector", 10)]
public class FieldSelectorNode : Node, ITypeNode
{
    [Input(connectionType = ConnectionType.Override)]
    public object target;

    [Output(dependencies = new[] { nameof(target) })]
    public object value;

    public FieldInfo selectedField;

    public Type ValueType
    {
        get => selectedField?.FieldType;
    }

    public bool HasRoot()
    {
        var port = GetPort(nameof(target));

        if (!port.IsConnected)
            return false;

        var node = port.GetConnection(0).node as ITypeNode;
        return node!.HasRoot();
    }

    public void SetValue(object value)
    {
        throw new NotImplementedException();
    }

    // Return the correct value of an output port when requested
    public override object GetValue(NodePort port)
    {
        if (selectedField == null)
            return null;

        target = GetInputValue(nameof(target), target);

        return target == null ? null : selectedField.GetValue(target);
    }

    public override void OnCreateConnection(NodePort @from, NodePort to)
    {
        base.OnCreateConnection(@from, to);

        if (to != GetInputPort(nameof(target)))
            return;

        if (@from.node is ITypeNode)
            return;
        
        to.Disconnect(@from);
    }
}