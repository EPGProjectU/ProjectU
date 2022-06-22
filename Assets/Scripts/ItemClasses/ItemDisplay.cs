using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ItemDisplay : MonoBehaviour
{
    public Item item;
    public EquipmentHandler equipmentHandler;

    public void Start()
    {
        
    }

    public void Update()
    {
        
    }

    public void ChangeText(Text text)
    {
        string output = item.ToString();
        text.text = output;

    }

}
