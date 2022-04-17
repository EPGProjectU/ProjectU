using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equipment : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    public Text text;

    //Method creates a string for all items in current Equipment for current instance of class
    public void ShowEquipment()
    {
        string testtext = "Ekwipunek: ";
        foreach (Item item in items)
        {
            testtext +="\n" +  item;
        }
        text.text = testtext;
    }

}
