using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyController : ActorController
{    
    public Transform currentTarget;  //change to private and calculate based on AI module
    private UnityEngine.AI.NavMeshAgent agent;

    void Start()
    {
        base.Setup();
        SetupAgent();
    }

    void Update() {
        UpdateAgent();
    }

    private void SetupAgent() {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.speed = BaseSpeed;
        agent.updateRotation = false; //rotation to face towards target will be handled by animation system
        agent.updateUpAxis = false;
    }

    private void UpdateAgent() {
        // Update agent destination if the target moves one unit
        if (Vector3.Distance(agent.destination, currentTarget.position) > 1.0f) {
            agent.destination = currentTarget.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
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
