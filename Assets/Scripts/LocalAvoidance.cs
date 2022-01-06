using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class LocalAvoidance : MonoBehaviour
{
    private Seeker seeker;

    void Start()
    {
        seeker = GetComponent<Seeker>();
    }

    void Update()
    {
        //raycast and if obstacle present modify waypoint(s) in seeker.Path.vectorPath
        //for now it is just colliders but that may be buggy 
    }
}
