using System;
using System.Collections.Generic;
using UnityEngine;
public enum DamageType
{
    Normal,
    Poison,
    Corrosion
}

[Serializable]
public struct DamageInfo
{
    public int damage;
    public int specialDamage;
    public float effectDuration;
    public DamageType type;

    public DamageInfo(int damage)
    {
        this.damage = damage;
        this.specialDamage = 0;
        this.effectDuration = 0;
        this.type = DamageType.Normal;
    }

    public DamageInfo(int damage, int specialDamage, float effectDuration ,DamageType type)
    {
        this.damage = damage;
        this.specialDamage = specialDamage;
        this.effectDuration = effectDuration;
        this.type = type;
    }

}
