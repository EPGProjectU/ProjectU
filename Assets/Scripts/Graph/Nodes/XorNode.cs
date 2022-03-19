using System.Linq;

/// <summary>
/// Checks if only one of inputs is true
/// </summary>
[CreateNodeMenuAttribute("Logic/Xor", 3)]
public class XorNode : BoolNode
{
    public override bool GetValue()
    {
        return GetInputValues("input", input).Count(v => v) == 1;
    }
}