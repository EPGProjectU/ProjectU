[CreateNodeMenuAttribute("Logic/And", 2)]
public class AndNode : BoolNode
{
    public override bool GetValue()
    {
        return MathHelper.And(GetInputValues("input", input));
    }
}