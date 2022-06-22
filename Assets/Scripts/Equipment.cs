using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Equipment : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    public Text text;

    private void Start()
    {
        SaveEventSystem.Instance.OnSaveData += Save;
        SaveEventSystem.Instance.OnLoadData += Load;
    }

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

    private void Save(GameData data)
    {
        foreach (Item item in items)
        {
            data.player.items.Add(AssetDatabase.GetAssetPath(item.GetInstanceID()));
        }
    }

    private void Load(GameData data)
    {
        foreach (string itemPath in data.player.items)
        {
            items.Add((Item)AssetDatabase.LoadAssetAtPath(itemPath, typeof(Item)));
        }
    }

    private void OnDestroy()
    {
        SaveEventSystem.Instance.OnSaveData -= Save;
        SaveEventSystem.Instance.OnLoadData -= Load;
    }

}
