using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public int health = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
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
