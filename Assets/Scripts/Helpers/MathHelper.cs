using System.Linq;

public static class MathHelper
{
    public static bool Or(bool[] input)
    {
        return input.Aggregate((current, value) => current | value);
    }

    public static bool And(bool[] input)
    {
        return input.Aggregate((current, value) => current & value);
    }
}