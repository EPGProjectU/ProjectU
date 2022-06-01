using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlot : MonoBehaviour
{
    public Ally Owner;
    public GameObject weapon;
    private GameObject fist;
    void Awake()
    {
        weapon = transform.GetChild(0).gameObject;
    }
    void Start()
    {
        if (weapon != null)
        {
            if (weapon.GetComponent<Collider2D>() == null) weapon.AddComponent<BoxCollider2D>();
            weapon.GetComponent<Collider2D>().enabled = false;
            if (weapon.GetComponent<WeaponDamager>() == null)
            {
                weapon.AddComponent<WeaponDamager>();
                weapon.GetComponent<WeaponDamager>().damage.damage = 1;
            }
            weapon.GetComponent<WeaponDamager>().Owner = Owner;
        }
        else
        {
            fist = new GameObject("Fist");
            fist.AddComponent<CircleCollider2D>().isTrigger = true;
            fist.GetComponent<CircleCollider2D>().radius = 0.5f;
            EquipWeapon(fist);
        }
    }

    //add pefab to actor
    public void EquipWeapon(GameObject weapon)
    {
        Destroy(this.weapon);
        weapon.transform.position = transform.position;
        weapon.transform.rotation = transform.rotation;
        weapon.transform.SetParent(transform);
        if (weapon.GetComponent<Collider2D>() == null) weapon.AddComponent<BoxCollider2D>().isTrigger = true;
        if (weapon.GetComponent<WeaponDamager>() == null)
        {
            weapon.AddComponent<WeaponDamager>();
            weapon.GetComponent<WeaponDamager>().damage.damage = 1;
        }
        weapon.GetComponent<Collider2D>().enabled = false;
        weapon.GetComponent<WeaponDamager>().Owner = Owner;
    }

    public void UpdateWeaponDamage(DamageInfo damage)
    {
        weapon.GetComponent<WeaponDamager>().damage = damage;
    }

    public void AddEffectWeaponDamage(int specialDamage, float effectDuration ,DamageType type)
    {
        weapon.GetComponent<WeaponDamager>().damage.specialDamage = specialDamage;
        weapon.GetComponent<WeaponDamager>().damage.effectDuration = effectDuration;
        weapon.GetComponent<WeaponDamager>().damage.type = type;
    }
}
