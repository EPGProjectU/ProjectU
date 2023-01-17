[CreateNodeMenu("BehaviourTree/Leaf/TaskPatrol")]
public class TaskPatrol : LeafNode
{
    [Input]
    public PatrolDefinition patrolDefinition;

    [Input]
    public int nextWaypoint;

    public override NodeState Evaluate(AIController controller)
    {
        Patrol(controller);
        state = NodeState.RUNNING;
        return state;
    }

    private void Patrol(AIController controller)
    {
        patrolDefinition = GetInputValue<PatrolDefinition>(nameof(patrolDefinition));

        nextWaypoint = GetInputValue<int>(nameof(nextWaypoint));

        if (patrolDefinition is PathPatrolDefinition pathPatrol)
        {
            nextWaypoint %= pathPatrol.path.Count;
            controller.agent.destination = pathPatrol.path[nextWaypoint];
            controller.agent.isStopped = false;

            if (controller.agent.remainingDistance <= controller.agent.stoppingDistance && !controller.agent.pathPending)
            {
                nextWaypoint++;
                var node = GetPort(nameof(nextWaypoint)).Connection.node;

                if (node is ParameterNode parameterNode)
                    parameterNode.SetValue(nextWaypoint);
            }
        }
    }
}