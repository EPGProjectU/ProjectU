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
        if (!isInvincible) base.TakeDamage(DefenceActsLikeHealth(damage));
    }

    protected DamageInfo DefenceActsLikeHealth(DamageInfo damage)
    {
        int dmg = damage.damage;
        if (defence > 0)
        {
            int tmp = dmg;
            dmg -= defence;
            defence -= tmp;
        }
        if (dmg < 0) dmg = 0;
        return new DamageInfo(dmg, damage);
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

    protected override void OnDeath()
    {
        base.OnDeath();
        Destroy(gameObject);
    }
}
