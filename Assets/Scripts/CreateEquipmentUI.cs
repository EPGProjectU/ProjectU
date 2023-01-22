using UnityEngine;
using UnityEngine.UI;

public class CreateEquipmentUI : MonoBehaviour
{
    // Start is called before the first frame update

    private Equipment equipment;
    public Image prefab;
    public EquipmentHandler equipmentHandler;

    public Text itemNameTextField;
    public Text itemDescriptionTextField;
    public Text itemUseTypeButtonTextField;
    public Text itemStatsTextField;
    public Image itemStatsIcon;
    //public Text itemUseSuccessMessage;

    public Image ArmorIcon;
    public Image WeaponIcon;
    public Image ConsumableIcon;
    public Image MiscIcon;

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
                image.gameObject.GetComponent<Button>().onClick.AddListener(() => { 
                    ChangeUIBasedOnItem(itemNameTextField, 
                        itemUseTypeButtonTextField, 
                        itemDescriptionTextField, 
                        itemStatsTextField, 
                        itemStatsIcon, item);
                    HighlightClickedItem();
                });
            }
        }
    }

    private void ChangeUIBasedOnItem(
        Text itemName, 
        Text buttonText, 
        Text descriptionText, 
        Text statsText, 
        Image typeIcon, 
        Item item)
    {
        itemName.text = item.Name;
        descriptionText.text = item.description;
        statsText.text = item.getStats();

        switch (item.GetType().ToString()) {
            case "Armor":
                buttonText.text = "Wear armor";
                typeIcon = ArmorIcon;
                break;
            case "Weapon":
                buttonText.text = "Equip weapon";
                typeIcon = WeaponIcon;
                break;
            case "Consumable":
                buttonText.text = "Drink";
                typeIcon = ConsumableIcon;
                break;
            case "Miscellaineous":
                buttonText.text = "Use";
                typeIcon = ConsumableIcon;
                break;
        }
        EquipmentHandler.selectedItem = item;
    }

    private void HighlightClickedItem() {
        //Just a gray square rendered "under" item would be nice, alpha channels ?
    }



    // Update is called once per frame
    void Update()
    {
        
    }
    void UpdateMenu()
    {

    }
}
