using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//rename to AIController ? 

public class EnemyController : ActorController
{

    Pathfinder pathfinder;
    //AIControlSystem AIControlSystem

    private Transform currentTarget;

    // Start is called before the first frame update
    void Start()
    {
        //AIControlSystem = GetComponent<AIControlSystem>
    }

    // Update is called once per frame
    void Update()
    {
        //AIControlSystem.UpdateTarget();
        UpdateTarget();

        UpdateVelocity(pathfinder.moveUsingPathfinding(this.gameObject.transform, currentTarget, this.BaseSpeed));
    }


    //part of AI 
    // update target every frame ?
    private void UpdateTarget() {
        currentTarget = GameObject.FindGameObjectWithTag("Player").transform; //temporary for test
    }
}
