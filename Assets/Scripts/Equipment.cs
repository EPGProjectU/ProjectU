using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    List<Item> items = new List<Item>();
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void ShowEquipment()
    {
        foreach (Item item in items)
        {
            Debug.Log(item.Name);
        }
    }

}
