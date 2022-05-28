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
            if (damage.type == DamageType.Poison && !isPoisoned)
            {
                isPoisoned = true;
                StartCoroutine(Poisoned(damage.effectDuration, damage.specialDamage));
            }
            else if (damage.type == DamageType.Corrosion && !isCorroding)
            {
                isCorroding = true;
                StartCoroutine(Corroding(damage.effectDuration, damage.specialDamage));
            }
        }
    }

    protected new IEnumerator Corroding(float time, int damage)
    {
        float duration = 0;
        while (time > duration)
        {
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
