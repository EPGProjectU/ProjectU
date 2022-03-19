/// <summary>
/// Negates single input
/// </summary>
[CreateNodeMenuAttribute("Logic/Not", 2)]
public class NotNode : BoolNode
{
    public override bool GetValue()
    {
        return !GetInputValue("input", input);
    }
}