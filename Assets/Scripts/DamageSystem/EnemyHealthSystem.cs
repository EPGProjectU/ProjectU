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
        SaveEventSystem.Instance.OnSaveData += Save;
        SaveEventSystem.Instance.OnLoadData += Load;
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
        return new DamageInfo(damage) { damage = dmg };
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
    }

    void OnDestroy()
    {
        SaveEventSystem.Instance.OnSaveData -= Save;
        SaveEventSystem.Instance.OnLoadData -= Load;
    }

    private void Save(GameData data)
    {
        EnemyData ed = new EnemyData();
        ed.name = gameObject.name;
        ed.health = health;
        ed.maxHealth = maxHealth;
        ed.armorDurability = armorDurability;
        ed.maximumArmorDurability = maximumArmorDurability;
        ed.position = gameObject.transform.position;
        ed.weapon = new DamageData(GetComponentInChildren<WeaponSlot>().weapon.GetComponent<WeaponDamager>().damage);
        data.enemies.Add(ed);
    }

    private void Load(GameData data)
    {
        EnemyData ed = null;
        
        foreach(EnemyData tmp in data.enemies)
        {
            if (tmp.name == gameObject.name) ed = tmp;
        }
        if (ed != null)
        {
            health = ed.health;
            maxHealth = ed.maxHealth;
            armorDurability = ed.armorDurability;
            maximumArmorDurability = ed.maximumArmorDurability;
            gameObject.transform.position = ed.position;
            GetComponentInChildren<WeaponSlot>().weapon.GetComponent<WeaponDamager>().damage = new DamageInfo(ed.weapon);

            if (health < 1) OnDeath();
        }
    }
}
