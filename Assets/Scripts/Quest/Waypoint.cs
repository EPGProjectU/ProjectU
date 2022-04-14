using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Waypoint : MonoBehaviour
{
    public Sprite Icon;
    public Color WaypointColor = new Color(1f, 1f, 1f, 1f);
    
    private GameObject iconObject;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
            Destroy(gameObject);
    }

    private void OnValidate()
    {
        if (iconObject == null)
        {

            iconObject = new GameObject("Waypoint Icon")
            {
                hideFlags = HideFlags.HideAndDontSave,
                transform =
                {
                    parent = transform,
                    localPosition = Vector3.zero
                }
                
            };

            iconObject.AddComponent<SpriteRenderer>();
        }
        
        iconObject.GetComponent<SpriteRenderer>().sprite = Icon;
        iconObject.GetComponent<SpriteRenderer>().color = WaypointColor;
    }
}
