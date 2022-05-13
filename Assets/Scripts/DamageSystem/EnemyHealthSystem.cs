using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyHealthSystem : HealthSystem
{
    public GameObject weapon;
    void Awake()
    {
        weapon = GetComponentsInChildren<Transform>().FirstOrDefault(c => c.gameObject.name == "WeaponSlot")?.gameObject.transform.GetChild(0).gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        allies.Add(Ally.Enemy);
        
        weapon.GetComponent<Collider2D>().enabled = false;
        if (weapon.GetComponent<WeaponDamager>() == null)
        {
            weapon.AddComponent<WeaponDamager>();
            weapon.GetComponent<WeaponDamager>().damage.damage = 1;
        }
        weapon.GetComponent<WeaponDamager>().Owner = Ally.Enemy;
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
