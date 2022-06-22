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

    protected new void OnDeath()
    {
        base.OnDeath();
        Destroy(gameObject);
    }
}
