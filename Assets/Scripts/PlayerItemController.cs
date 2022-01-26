using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemController : MonoBehaviour
{
    public EquipmentController equipmentController;
    private int itemsInTrigger = 0;
    private int mockupInt = 0;
    private List<Item> items = new List<Item>();

    private void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            UnityEngine.Debug.Log("2");
            if (itemsInTrigger > 0)
            {
                UnityEngine.Debug.Log("3");
                equipmentController.PickupSingle(items[0]);
                if (mockupInt == 0)
                {
                    items = new List<Item>();
                    mockupInt++;
                }
                UnityEngine.Debug.Log("4");
                UnityEngine.Debug.Log("5");
                UnityEngine.Debug.Log("6");
            }
            UnityEngine.Debug.Log("7");
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //StartCoroutine(ItemPickupZone(collision.GetComponent<Item>()));
        if (collision.tag == "Item")
        {
            UnityEngine.Debug.Log("Test");
            itemsInTrigger += 1;
            items.Add(collision.GetComponent<Item>());
            UnityEngine.Debug.Log("1");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //StopCoroutine(ItemPickupZone(collision.GetComponent<Item>()));
        if (collision.tag == "Item")
        {
            Item itemOnGround = collision.GetComponent<Item>();
            if (items.Find(i => i.name == itemOnGround.name))
            {
                items.Remove(itemOnGround);
                itemsInTrigger -= 1;
            }
            UnityEngine.Debug.Log("8");
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
