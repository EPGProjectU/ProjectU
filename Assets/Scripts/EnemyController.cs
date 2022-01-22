using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyController : ActorController
{
    protected Pathfinder pathfinder;
   
    public Transform currentTarget;

    void Start()
    {
        base.Setup();
        pathfinder = new Pathfinder(GetComponent<Seeker>());
    }

    void Update() {

        //pathfinder calculates path and next vel vector based on the path
        UpdateVelocity(pathfinder.moveUsingPathfinding(transform, currentTarget, this.BaseSpeed));
    }

}
