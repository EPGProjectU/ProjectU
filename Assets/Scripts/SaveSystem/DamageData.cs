using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class DamageData
{
    public int damage;
    public int specialDamage;
    public float effectDuration;
    public DamageType type;

    public float force;
    public float distance;
    public float recoveryTime;

    public DamageData() { }

    public DamageData(DamageInfo info)
    {
        damage = info.damage;
        specialDamage = info.specialDamage;
        effectDuration = info.effectDuration;
        type = info.type;
        force = info.knockBack.force;
        distance = info.knockBack.distance;
        recoveryTime = info.knockBack.recoveryTime;
    }
}