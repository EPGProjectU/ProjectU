using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Pathfinder
{

    private Path path;
    private Seeker seeker;
    private bool reachedDest;

    private float nextWaypointDistance = 1;
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

    public Vector2 moveUsingPathfinding(Transform current, Transform target, float speed) {

        seeker.StartPath(current.position, target.position, OnPathComplete);   //every time calculates new path could be optimized
       

        if (path == null) {
            return Vector3.zero;
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
        //current.position += velocity * Time.deltaTime;
        //Debug.Log(velocity);

        return new Vector2(velocity.x, velocity.y);

    }

    public Vector3 moveUsingPathfindingVec3(Transform current, Transform target, float speed) {

        seeker.StartPath(current.position, target.position, OnPathComplete);   //every time calculates new path could be optimized


        if (path == null) {
            return Vector3.zero;
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
        //current.position += velocity * Time.deltaTime;
        //Debug.Log(velocity);

        return velocity;

    }

    private void OnPathComplete(Path p) {
        if (!p.error) {
            path = p;
            currentWaypoint = 0;
        }
    }
}
