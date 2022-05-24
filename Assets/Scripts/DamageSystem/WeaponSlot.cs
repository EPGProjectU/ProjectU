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
        fist = new GameObject("Fist");
        fist.AddComponent<CircleCollider2D>().isTrigger = true;
        fist.GetComponent<CircleCollider2D>().radius = 0.5f;
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
            EquipWeapon(fist);
        }
    }

    public void EquipWeapon(GameObject weapon)
    {
        Debug.Log(transform.position+" "+transform.rotation);
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
}
