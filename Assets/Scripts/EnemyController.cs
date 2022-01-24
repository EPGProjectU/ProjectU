using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyController : ActorController
{
    protected Pathfinder pathfinder;
    //AISystem AISystem = new AISystem(BehaviourTree); 

    //temporary testing behaviour of enemy is chasing target
    public Transform currentTarget;
    bool playerDetected = false;

    void Start()
    {
        base.Setup();
        //pathfinder needs Seeker component to work
        pathfinder = new Pathfinder(GetComponent<Seeker>());
    }

    void Update() {

        //AISystem.updateActor();

        //pathfinder calculates path and then next velocity vector based on the calculated path and progress on it
        //Vector2 newVelocity = pathfinder.moveAlongPath(transform, currentTarget, this.BaseSpeed);
        //UpdateVelocity(newVelocity);

        //#region aiDetectionMashup
        //temporary player detection prototype, every 5 frames for perfromance
        if(Time.frameCount % 5 == 0)
            playerDetected = checkIfPlayerVisible();

        if (playerDetected) {
            Vector2 newVelocity = pathfinder.moveAlongPath(transform, currentTarget, this.BaseSpeed);
            UpdateVelocity(newVelocity);
        }
    }

    private bool checkIfPlayerVisible() {
        //if(Debug) smh true
        //DrawRay(this.transform.position, currentTarget.position, Color.black, 10f, false);

        RaycastHit2D objectHit = Physics2D.Raycast(this.transform.position, currentTarget.position - transform.position, 
                                                    Mathf.Infinity, 0, -Mathf.Infinity, Mathf.Infinity);

        Debug.Log(objectHit.collider.gameObject.name);

        if (objectHit.collider.gameObject.name == "Player")
            return true;
        else
            return false;
    }

        //#endregion aiDetectionMashup

}
