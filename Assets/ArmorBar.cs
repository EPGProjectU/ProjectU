using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmorBar : MonoBehaviour
{
    private float max_Armor = 100.0f;
    public PlayerHealthSystem armor;
    private Image armorBar;

    // Start is called before the first frame update
    void Start()
    {
        armor = GameObject.Find("Player").GetComponent<PlayerHealthSystem>();
        armorBar = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        armorBar.fillAmount = armor.armorDurability / max_Armor;
    }
}
