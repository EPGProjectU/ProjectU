using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemController : MonoBehaviour
{
    public EquipmentController equipmentController;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(ItemPickupZone(collision.GetComponent<Item>()));
        Debug.Log("Test");
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        StopCoroutine(ItemPickupZone(collision.GetComponent<Item>()));
    }


    IEnumerator ItemPickupZone(Item item)
    {
        while (true)
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
