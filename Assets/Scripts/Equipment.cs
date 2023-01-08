using System;
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
        List<String> tmpSave = new List<String>();
        foreach (Item item in items)
        {
            tmpSave.Add(AssetDatabase.GetAssetPath(item.GetInstanceID()));
        }
        data.player.items = tmpSave;
    }

    private void Load(GameData data)
    {
        List<Item> tmp = new List<Item>();
        foreach (string itemPath in data.player.items)
        {
            tmp.Add((Item)AssetDatabase.LoadAssetAtPath(itemPath, typeof(Item)));
        }
        items = tmp;
    }

    private void OnDestroy()
    {
        SaveEventSystem.Instance.OnSaveData -= Save;
        SaveEventSystem.Instance.OnLoadData -= Load;
    }

}
