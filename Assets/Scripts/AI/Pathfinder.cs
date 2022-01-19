using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Pathfinder
{

    public Path path;
    private Seeker seeker;
    public float speed = 2;
    public bool reachedDest;

    public float nextWaypointDistance = 3;
    private int currentWaypoint = 0;

    public Pathfinder(Seeker seeker) {
        this.seeker = seeker;

    }

/*    public void calculatePath(Seeker seeker, Transform start, Transform target) {
        seeker.pathCallback = OnPathComplete;
        seeker.StartPath(start.position, target.position);
    }

    private void OnPathComplete(Path path) {
        Debug.Log("Yay, we got a path back. Did it have an error? " + path.error);
        if (!path.error) {
            this.path = path;
            currentWaypoint = 0;
        }
    }*/

  /*  public void traversePath(Path p) {
        if (p == null) {
            return;
        }


        reachedDest = false;
        float distanceToWaypoint;

        while (true) {
            distanceToWaypoint = Vector3.Distance(transform.position, p.vectorPath[currentWaypoint]);

            if (distanceToWaypoint < nextWaypointDistance) {
                if (currentWaypoint + 1 < path.vectorPath.Count) {
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
    }*/




  /*  public moveOnPath(Path p, float speed) {
        calculatePath(); //check if path changed before moving, performance intensive 


    }*/

    public void moveUsingPathfinding(Transform current, Transform target, float speed) {

        seeker.StartPath(current.position, target.position, OnPathComplete);   //every time calculates new path could be optimized
        void OnPathComplete(Path p) {
            if (!path.error) {
                path = p;
                currentWaypoint = 0;
            }
        }

        if (path == null) {
            return;
        }

        reachedDest = false;
        float distanceToWaypoint;

        while (true) {
            distanceToWaypoint = Vector3.Distance(current.position, path.vectorPath[currentWaypoint]);

            if (distanceToWaypoint < nextWaypointDistance) {
                if (currentWaypoint + 1 < path.vectorPath.Count) {
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

        Vector3 dir = (path.vectorPath[currentWaypoint] - current.position).normalized;
        Vector3 velocity = dir * speed * speedFactor;

        //controller.move(velocity)
        current.position += velocity * Time.deltaTime;

    }
}
