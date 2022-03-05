/// <summary>
/// Checks if any of inputs are true
/// </summary>
[CreateNodeMenuAttribute("Logic/Or", 1)]
public class OrNode : BoolNode
{
    public override bool GetValue()
    {
        return MathHelper.Or(GetInputValues("input", input));
    }
}