using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyController : ActorController
{
    protected Pathfinder pathfinder;
    
    //temporary testing behaviour of enemy is chasing target
    public Transform currentTarget;

    void Start()
    {
        base.Setup();
        //pathfinder needs Seeker component to work
        pathfinder = new Pathfinder(GetComponent<Seeker>());
    }

    void Update() {

        //pathfinder calculates path and then next velocity vector based on the calculated path and progress on it
        Vector2 newVelocity = pathfinder.moveAlongPath(transform, currentTarget, this.BaseSpeed);
        UpdateVelocity(newVelocity);
    }
}
