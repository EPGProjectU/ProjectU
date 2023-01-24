using ProjectU.Core;
using UnityEngine;

public class ItemSearcher : MonoBehaviour
{
    public static ItemInfo findClosestItem(Vector3 position)
    {
        GameObject closestObject = null;
        var closestDistance = -1f ;
        foreach(var item in TagList._FindGameObjectsWithTag("Item"))
        {
            float dist = Vector2.Distance(item.transform.position, position);
            if(closestDistance == -1f || closestDistance > dist)
            {
                closestDistance = dist;
                closestObject = item;
            }
        }
        return new ItemInfo(closestDistance, closestObject);
    }

}
