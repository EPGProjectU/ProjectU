using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData 
{
    public float health;
    public float maxHealth;
    public float armorDurability;
    public float maximumArmorDurability;
    public DamageInfo weapon;
    public Vector3 position;
    public List<String> items = new List<String>();
}