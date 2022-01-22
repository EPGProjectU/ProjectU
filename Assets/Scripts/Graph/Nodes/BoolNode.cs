using XNode;

public abstract class BoolNode : Node
{
    [Input]
    public bool input;

    [Output(dependencies = new[] { "input" })]
    public bool output;

    public override object GetValue(NodePort port)
    {
        if (port.fieldName == "output")
            return GetValue();

        return null;
    }

    public abstract bool GetValue();
}