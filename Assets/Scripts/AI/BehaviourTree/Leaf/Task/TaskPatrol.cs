using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateNodeMenu("BehaviourTree/Leaf/TaskPatrol")]
public class TaskPatrol : LeafNode
{
    //public List<Transform> waypointList;

    //
    

    public TaskPatrol() { }

    public override NodeState Evaluate(AIController controller) {
        Patrol(controller);
        state = NodeState.RUNNING;
        return state;
    }
    
    private void Patrol(AIController controller) {
        controller.agent.destination = controller.wayPointList[controller.nextWayPoint].position;
        controller.agent.isStopped = false;


        if (controller.agent.remainingDistance <= controller.agent.stoppingDistance && !controller.agent.pathPending) {
            controller.nextWayPoint = (controller.nextWayPoint + 1) % controller.wayPointList.Count;
        }
    }
}
