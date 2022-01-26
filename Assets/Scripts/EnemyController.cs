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
    //

    public TagHook tagHook;

    void Start()
    {
        base.Setup();
        //pathfinder needs Seeker component added to actor to work
        //pathfinder needs Seeker component to work
        pathfinder = new Pathfinder(GetComponent<Seeker>());
    }

    void Update() {

        //AISystem.updateActor();
        //pathfinder calculates path and then next velocity vector based on the calculated path and progress on it
        //Vector2 newVelocity = pathfinder.moveAlongPath(transform, currentTarget, this.BaseSpeed);
        //UpdateVelocity(newVelocity);

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
        //UnityEngine.Debug.DrawRay(this.transform.position, currentTarget.position - transform.position, Color.black, 0f, false);

        RaycastHit2D objectHit = Physics2D.Raycast(this.transform.position, currentTarget.position - transform.position);

        //if (objectHit)
        //UnityEngine.Debug.Log(objectHit.collider.gameObject.name);

        if (objectHit && objectHit.collider.gameObject.name == "Player")
            return true;
        else if (playerDetected) //once detected player will be detected forever currently
            return true;
        else 
            //UnityEngine.Debug.Log("Nothing hit :/");
            return false;
        
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
        if (health < 1) {
            Destroy(gameObject);
            tagHook?.Collect();
        }
    }
}
