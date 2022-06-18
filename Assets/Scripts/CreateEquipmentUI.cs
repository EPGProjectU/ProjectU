using System.Collections;
using System.Collections.Generic;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.UI;

public class CreateEquipmentUI : MonoBehaviour
{
    // Start is called before the first frame update

    private Equipment equipment;
    public Image prefab;
    public EquipmentHandler equipmentHandler;
    public Text text;
    
    void Start()
    {
        equipment = ProjectU.Core.TagList._FindGameObjectsWithTag("Player")[0].GetComponent<Equipment>();
        Image image;
        if(equipment.items != null)
        {
            foreach (Item item in equipment.items)
            {
                image = (Image)Instantiate(prefab, transform);
                image.GetComponent<Image>().sprite = item.sprite;
                ItemDisplay id = image.gameObject.AddComponent<ItemDisplay>();
                id.item = item;
                id.equipmentHandler = equipmentHandler;
                image.gameObject.GetComponent<Button>().onClick.AddListener(() => { ChangeText(text, item); });

            }
        }
    }

    void ChangeText(Text text, Item item)
    {
        string output = item.ToString();
        text.text = output;
        EquipmentHandler.selectedItem = item;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void UpdateMenu()
    {

    }
}
