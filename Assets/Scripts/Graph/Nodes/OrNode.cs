using System.Linq;

/// <summary>
/// Checks if any of inputs are true
/// </summary>
[CreateNodeMenuAttribute("Logic/Or", 1)]
public class OrNode : BoolNode
{
    public override bool GetValue()
    {
        return GetInputValues("input", input).Any(b => b);
    }
}