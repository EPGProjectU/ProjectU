using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    public List<Item> items = new List<Item>();


    public void ShowEquipment()
    {
        foreach (Item item in items)
        {
            Debug.Log(item.Name);
        }
    }

}
