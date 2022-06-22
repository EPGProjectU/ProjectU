using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerHealthSystem : HealthSystem
{
    void Awake()
    {
        GetComponentInChildren<WeaponSlot>().Owner = myGroup;
    }
    // Start is called before the first frame update
    void Start()
    {
        allies.Add(myGroup);
        SaveEventSystem.Instance.OnSaveData += Save;
        SaveEventSystem.Instance.OnLoadData += Load;
    }
   
    /// <summary>
    /// Calculate amount of damage that will be taken by Player if he isn't invincilbe and when health drops below 1 uses OnDeath
    /// </summary>
    public override void TakeDamage(DamageInfo damage)
    {
        if (!isInvincible) base.TakeDamage(AbsorbDamage(damage));
    }

    protected DamageInfo AbsorbDamage(DamageInfo damage)
    {
        int dmg = damage.damage;
        if (armorDurability > 0)
        {
            dmg -= defence;
            if (dmg < 0) armorDurability += dmg;
            else armorDurability -= defence;
            if (armorDurability < 0) armorDurability = 0;
        }
        if (dmg < 0) dmg = 0;
        return new DamageInfo(dmg, damage);
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        Debug.Log("Player is Dead");
        SaveEventSystem.Instance.OnSaveData -= Save;
        SaveEventSystem.Instance.OnSaveData -= Load;
    }

    private void Save(GameData data)
    {
        data.player.health = health;
        data.player.maxHealth = maxHealth;
        data.player.armorDurability = armorDurability;
        data.player.maximumArmorDurability = maximumArmorDurability;
        data.player.position = gameObject.transform.position;
    }

    private void Load(GameData data)
    {
        health = data.player.health;
        maxHealth = data.player.maxHealth;
        armorDurability = data.player.armorDurability;
        maximumArmorDurability = data.player.maximumArmorDurability;
        gameObject.transform.position = data.player.position;
    }

}
