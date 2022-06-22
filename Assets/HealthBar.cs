using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public PlayerHealthSystem health;
    private Image healthBar;
    
    // Start is called before the first frame update
    void Start()
    {
        health = GameObject.Find("Player").GetComponent<PlayerHealthSystem>();
        healthBar = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = health.health / health.maxHealth;
    }
}
