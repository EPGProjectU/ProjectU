using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleChaser : MonoBehaviour
{
    Pathfinder pathfinder;

    public Transform target;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        pathfinder = new Pathfinder(GetComponent<Seeker>());
    }

    // Update is called once per frame
    void Update()
    {
        //vel = pathfinder.moveUsingPathfinding()
        //controller.move(vel)
        transform.position += pathfinder.moveUsingPathfinding(this.transform, target, speed) * Time.deltaTime;
       
    }
}
