using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterData
{
    public float health;
    public float maxHealth;
    public float armorDurability;
    public float maximumArmorDurability;
    public DamageInfo weapon;
    public Vector3 position;
}