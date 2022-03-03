using System;
using System.Linq;
using XNode;

[CreateNodeMenu("Progression/Lock", 2)]
public class LockNode: Node
{
    [Input]
    public bool inLock;

    [Input]
    public bool inUnlock;

    [Output(dependencies = new[] { "inLock", "inUnlock" })]
    public bool output;

    public enum Quantifier
    {
        Any = 0,
        All = 1
    }

    public Quantifier lockQuantifier = Quantifier.Any;

    public Quantifier unlockQuantifier = Quantifier.All;

    // Return the correct value of an output port when requested
    public override object GetValue(NodePort port)
    {
        if (port.fieldName == "output")
            return !IsLocked();

        return null;
    }

    public bool IsLocked()
    {
        return CheckLocks() && !CheckUnlocks();
    }

    public bool CheckLocks()
    {
        var values = GetInputValues("inLock", inLock);

        return lockQuantifier switch
        {
            Quantifier.Any => values.Any(b => b),
            Quantifier.All => values.All(b => b),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public bool CheckUnlocks()
    {
        var values = GetInputValues("inUnlock", inUnlock);

        return unlockQuantifier switch
        {
            Quantifier.Any => values.Any(b => b),
            Quantifier.All => values.All(b => b),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}