using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equipment : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    public Text text;


    public void ShowEquipment()
    {
        string testtext = "Ekwipunek: ";
        foreach (Item item in items)
        {
            testtext += '\n' + (item.Name);
        }
        text.text = testtext;
    }

}
