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
        //could be earlyupdate or late to make sure it is called at ther right time

        //raycast and if obstacle present modify waypoint(s) in seeker.Path.vectorPath
        //for now it is just collider physics but that may be buggy

        //https://docs.unity3d.com/ScriptReference/Collider.Raycast.html
        //https://howtorts.github.io/2014/01/14/avoidance-behaviours.html
    }
}
