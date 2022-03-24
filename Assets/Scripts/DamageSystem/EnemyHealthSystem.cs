using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthSystem : HealthSystem
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void TakeDamage(DamageInfo damage)
    {
        health -= damage.damage;
        if (health < 1) OnDeath();
    }

    protected override void OnDeath()
    {
        Destroy(gameObject);
    }
}
