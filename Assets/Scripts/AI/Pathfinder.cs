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

        return new Vector2(velocity.x, velocity.y);

    }

    private void OnPathComplete(Path p) {
        if (!p.error) {
            path = p;
            currentWaypoint = 0;
        }
    }
}
