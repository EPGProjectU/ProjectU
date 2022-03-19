using XNode;

/// <summary>
/// Base for logic nodes
/// </summary>
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

    /// <summary>
    /// Function to be overloaded in subsequent inherent nodes
    /// </summary>
    /// <returns>Output value of logic arithmetics</returns>
    public abstract bool GetValue();
}