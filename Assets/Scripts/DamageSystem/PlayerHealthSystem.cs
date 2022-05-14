using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerHealthSystem : HealthSystem
{
    
    public GameObject weapon;
    

    void Awake()
    {
        weapon = GetComponentsInChildren<Transform>().FirstOrDefault(c => c.gameObject.name == "WeaponSlot")?.gameObject.transform.GetChild(0).gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        allies.Add(Ally.Player);
        isInvincible = false;
        weapon.GetComponent<Collider2D>().enabled = false;
        if (weapon.GetComponent<WeaponDamager>() == null)
        {
            weapon.AddComponent<WeaponDamager>();
            weapon.GetComponent<WeaponDamager>().damage.damage = 1;
        }
        weapon.GetComponent<WeaponDamager>().Owner = Ally.Player;
    }
   
    /// <summary>
    /// Calculate amount of damage that will be taken by Player if he isn't invincilbe and when health drops below 1 uses OnDeath
    /// </summary>
    public override void TakeDamage(DamageInfo damage)
    {
        if (!isInvincible)
        {
            isInvincible = true;
            health -= damage.damage;
            Debug.Log("Health = " + health);
            if (health < 1) OnDeath();
            StartCoroutine(InvincibleTimer());
        }
    }
    protected new void OnDeath()
    {
        base.OnDeath();
        gameObject.GetComponent<PlayerController>().enabled = false;
        Debug.Log("Player is Dead");
    }

    

}
