using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class ChaserAItest : MonoBehaviour
{
    public Transform targetPosition;
    private Transform oldTargetPosition;
    public Path path;
    Seeker seeker;
    public float speed = 2;
    public bool reachedDest;

    public float nextWaypointDistance = 3;
    private int currentWaypoint = 0;

    //private EnemyController controller;

    void Start()
    {
         oldTargetPosition = targetPosition;
         seeker = GetComponent<Seeker>();
         //calculatePath();
    }

    private void calculatePath() {
        seeker.StartPath(transform.position, targetPosition.position, OnPathComplete);
    }
        

    public void OnPathComplete (Path p) {
        //Debug.Log("Yay, we got a path back. Did it have an error? " + p.error);

        if (!p.error) {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void Update() {
        calculatePath();
       /* if (oldTargetPosition != targetPosition)
            calculatePath();*/

        if (path == null) {
            return;
        }

        reachedDest = false;
        float distanceToWaypoint;

        while(true) {
            distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);

            if(distanceToWaypoint < nextWaypointDistance) {
                if(currentWaypoint + 1 < path.vectorPath.Count) {
                    currentWaypoint++;
                }
                else {
                    reachedDest = true;
                    break;
                }
            }
            else {
                break;
            }
        }

        var speedFactor = reachedDest ? Mathf.Sqrt(distanceToWaypoint / nextWaypointDistance) : 1f;


        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        Vector3 velocity = dir * speed * speedFactor;

        //controller.move(velocity)
        transform.position += velocity * Time.deltaTime;

    }



}
