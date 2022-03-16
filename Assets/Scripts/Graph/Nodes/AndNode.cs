using System.Linq;

/// <summary>
/// Checks if all inputs are true
/// </summary>
[CreateNodeMenuAttribute("Logic/And", 2)]
public class AndNode : BoolNode
{
    public override bool GetValue()
    {
        return GetInputValues("input", input).All(b => b);
    }
}