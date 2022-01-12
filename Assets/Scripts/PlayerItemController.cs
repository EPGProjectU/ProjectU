using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemController : MonoBehaviour
{
    public EquipmentController equipmentController;
    private int itemsInTrigger;
    private List<Item> items = new List<Item>();

    private void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (itemsInTrigger > 0)
            {
                equipmentController.PickupSingle(items[0]);
                itemsInTrigger--;
                items.RemoveAt(0);
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //StartCoroutine(ItemPickupZone(collision.GetComponent<Item>()));
        UnityEngine.Debug.Log("Test");
        itemsInTrigger += 1;
        items.Add(collision.GetComponent<Item>());
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //StopCoroutine(ItemPickupZone(collision.GetComponent<Item>()));
        Item itemOnGround = collision.GetComponent<Item>();
        if (items.Find(i => i.name == itemOnGround.name))
        {
            items.Remove(itemOnGround);
            itemsInTrigger -= 1;
        }
    }


    IEnumerator ItemPickupZone(Item item)
    {
        while (!Input.GetButtonDown("Interact"))
        {
            if (Input.GetButtonDown("Interact"))
            {
                equipmentController.PickupSingle(item);
                break;
            }
            yield return new WaitForSeconds(0.02f);
        }
    }
}
