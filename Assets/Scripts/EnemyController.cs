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
        health = 2;
        //pathfinder needs Seeker component to work
        pathfinder = new Pathfinder(GetComponent<Seeker>());
    }

    void Update() {

        //pathfinder calculates path and then next velocity vector based on the calculated path and progress on it
        Vector2 newVelocity = pathfinder.moveAlongPath(transform, currentTarget, this.BaseSpeed);
        UpdateVelocity(newVelocity);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(1);
        }
        if (collision.gameObject.CompareTag("PlayersWeapon"))
        {
            TakeDamage(collision.gameObject.GetComponent<DamageInfo>().damage);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health < 1) Destroy(gameObject);
    }
}
