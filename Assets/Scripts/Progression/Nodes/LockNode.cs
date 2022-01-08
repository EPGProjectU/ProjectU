using System.Linq;
using XNode;

[CreateNodeMenu("Progression/Lock", 2)]
public class LockNode : Node
{
    [Input]
    public bool inLock;

    [Input]
    public bool inUnlock;

    [Output(dependencies = new[] { "inLock", "inUnlock" })]
    public bool output;

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

        return values.Aggregate(false, (current, value) => current | value);
    }

    public bool CheckUnlocks()
    {
        var values = GetInputValues("inUnlock", inUnlock);

        return values.Aggregate(true, (current, value) => current & value);
    }
}