using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSearcher : MonoBehaviour
{


    public static ItemInfo findClosestItem()
    {
        Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        GameObject closestObject = null;
        float closestDistance = -1f ;
        foreach(GameObject item in GameObject.FindGameObjectsWithTag("Item"))
        {
            float dist = Vector3.Distance(item.transform.position, playerPosition);
            if(closestDistance == -1f || closestDistance > dist)
            {
                closestDistance = dist;
                closestObject = item;
            }
        }
        return new ItemInfo(closestDistance, closestObject);
    }

}
