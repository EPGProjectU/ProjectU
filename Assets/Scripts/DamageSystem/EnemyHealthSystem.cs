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
    }

    public override void TakeDamage(DamageInfo damage)
    {
        if (!isInvincible)
        {
            int dmg = damage.damage;
            isInvincible = true;
            if (defence > 0)
            {
                int tmp = dmg;
                dmg -= defence;
                defence -= tmp;
            }
            if (dmg > 0) health -= dmg;
            if (health < 1) OnDeath();
            else StartCoroutine(InvincibleTimer());
            ApplySpecialEffect(damage);
        }
    }

    protected override IEnumerator Corroding(float time, int damage)
    {
        float duration = 0;
        while (time > duration)
        {
            if (defence - damage < 0)
            {
                defence = 0;
                break;
            }
            defence -= damage;
            yield return new WaitForSeconds(1);
            duration++;
        }
        isCorroding = false;
    }

    protected new void OnDeath()
    {
        base.OnDeath();
        Destroy(gameObject);
    }
}
