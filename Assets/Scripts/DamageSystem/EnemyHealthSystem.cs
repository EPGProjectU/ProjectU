using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyHealthSystem : HealthSystem
{
    void Awake()
    {
        GetComponentInChildren<WeaponSlot>().Owner = myGroup;
    }

    // Start is called before the first frame update
    void Start()
    {
        allies.Add(myGroup);
        isInvincible = false;
    }
    public override void TakeDamage(DamageInfo damage)
    {
        if (!isInvincible)
        {
            isInvincible = true;
            health -= damage.damage;
            if (health < 1) OnDeath();
            StartCoroutine(InvincibleTimer());
        }
    }

    protected new void OnDeath()
    {
        base.OnDeath();
        Destroy(gameObject);
    }
}
