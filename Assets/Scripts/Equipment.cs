using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equipment : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    public Text text;
    private int rigged = -1;


    public void ShowEquipment()
    {
        string testtext = "Ekwipunek: ";
        if (rigged >= 0)
        {
            foreach (Item item in items)
            {
                testtext += '\n' + (item.Name);
            }
            text.text = testtext;
        }
        else
        {
            rigged++;
        }
    }

}
