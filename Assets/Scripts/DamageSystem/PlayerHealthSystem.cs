using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerHealthSystem : HealthSystem
{
    private bool isInvincible;
    public GameObject weapon;
    public float invincibleTime = 0.5f;

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

        SaveEventSystem.Instance.OnSaveData += SaveGame;
        SaveEventSystem.Instance.OnLoadData += LoadGame;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
    /// <summary>
    /// Calculate amount of damage that will be taken by Player if he isn't invincilbe and when health drops below 1 uses OnDeath
    /// </summary>
    public override void TakeDamage(DamageInfo damageinfo)
    {
        if (!isInvincible)
        {
            isInvincible = true;
            health -= damageinfo.damage;
            Debug.Log("Health = " + health);
            if (health < 1) OnDeath();
            StartCoroutine(InvincibleTimer());
        }
    }
    protected override void OnDeath()
    {
        GetComponent<PlayerController>().IsDead = true;
        Debug.Log("Player is Dead");
    }

    void SaveGame(GameData data)
    {
        data.player.position = transform.position;
        data.player.health = health;
    }

    void LoadGame(GameData data)
    {
        transform.position = data.player.position;
        if (data.player.health > 0) health = data.player.health;
        else GetComponent<PlayerController>().IsDead = true;

    }

    IEnumerator InvincibleTimer()
    {
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }

    public void OnDestroy()
    {
        SaveEventSystem.Instance.OnLoadData -= LoadGame;
        SaveEventSystem.Instance.OnSaveData -= SaveGame;
    }

}
