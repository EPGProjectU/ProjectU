using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class TaskPatrol : BehaviourNode
{

    public TaskPatrol() { }

    public override NodeState Evaluate(EnemyController controller) {
        Patrol(controller);
        state = NodeState.RUNNING;
        return state;
    }
    
    private void Patrol(EnemyController controller) {
        controller.agent.destination = controller.wayPointList[controller.nextWayPoint].position;
        controller.agent.isStopped = false;


        if (controller.agent.remainingDistance <= controller.agent.stoppingDistance && !controller.agent.pathPending) {
            controller.nextWayPoint = (controller.nextWayPoint + 1) % controller.wayPointList.Count;
        }
    }
}
