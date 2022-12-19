using XNode;


public enum NodeState
{
    RUNNING,
    SUCCESS,
    FAILURE
}

[CreateNodeMenu("")]
public abstract class BehaviourNode : Node
{
    protected NodeState state;

    [Input]
    public BehaviourNode parent;

    public abstract NodeState Evaluate(AIController controller);
}