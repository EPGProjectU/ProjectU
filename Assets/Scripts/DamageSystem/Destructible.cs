using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : HealthSystem
{
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
