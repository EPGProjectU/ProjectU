using System;
using System.Linq;
using XNode;

public interface NotifyNodeInterface
{
    [Serializable]
    public class EmptyPort {}

    public bool Notify(object payload);
}

public static class NotifyNodeHelper
{
    public static bool SendNotify(NodePort port, object payload)
    {
        return port.GetConnections()
            .Select(connectedPort => connectedPort?.node)
            .OfType<NotifyNodeInterface>()
            .Aggregate(false, (current, node) => current | node.Notify(payload));
    }
}