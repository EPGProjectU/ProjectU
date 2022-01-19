using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//rename to AIController ? 

public class EnemyController : ActorController
{


    protected Pathfinder pathfinder;
    //AIControlSystem AIControlSystem
    protected Transform currentTarget;

    // Start is called before the first frame update
    void Start()
    {
        base.Setup();
        pathfinder = new Pathfinder(GetComponent<Seeker>());
        //AIControlSystem = GetComponent<AIControlSystem> //AiControlSystem is a script 
    }

    // Update is called once per frame
    void Update()
    {
        //AIControlSystem.UpdateTarget();
        UpdateTarget();

        UpdateVelocity(pathfinder.moveUsingPathfinding(transform, currentTarget, this.BaseSpeed));
    }


    //part of AI 
    // update target every frame ?
    private void UpdateTarget() {
        currentTarget = GameObject.FindGameObjectWithTag("Player").transform; //temporary for test
    }

  
      
}
