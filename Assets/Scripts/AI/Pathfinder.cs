using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Pathfinder
{

    private Path path;
    private Seeker seeker;
    private bool reachedDest;

    private float nextWaypointDistance = 1;  //ability to skip waypoints 
    private int currentWaypoint = 0;

    public Pathfinder(Seeker seeker, float nextWaypointDistance = 1) {
        this.seeker = seeker;

    }

    public Vector2 moveAlongPath(Transform current, Transform target, float speed) {

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

   /* private virtual void CalculateNextRotation(float slowdown, out Quaternion nextRotation) {
        if (lastDeltaTime > 0.00001f && enableRotation) {
            Vector2 desiredRotationDirection;
            desiredRotationDirection = velocity2D;

            // Rotate towards the direction we are moving in.
            // Don't rotate when we are very close to the target.
            var currentRotationSpeed = rotationSpeed * Mathf.Max(0, (slowdown - 0.3f) / 0.7f);
            nextRotation = SimulateRotationTowards(desiredRotationDirection, currentRotationSpeed * lastDeltaTime);
        }
        else {
            // TODO: simulatedRotation
            nextRotation = rotation;
        }
    }*/
}
