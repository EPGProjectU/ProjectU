using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public int health = 1;
    private bool isHit;
    // Start is called before the first frame update
    void Start()
    {
        isHit = false;
        DamageEventSystem.Instance.OnEnemyTakesDamage += TakeDamage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        DamageEventSystem.Instance.OnPlayerTakesDamage -= TakeDamage;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayersWeapon"))
        {
            isHit = true;
            DamageEventSystem.Instance.PlayerDealsDamage();
            //Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isHit)
        {
            health -= damage;
            if (health < 1) Destroy(gameObject);
            isHit = false;
        }
    }
}
