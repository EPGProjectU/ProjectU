using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentHandler : MonoBehaviour
{
    // Start is called before the first frame update

    public Text text;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeText(Item item)
    {
        string output = item.ToString();
        text.text = output;
        
    }
}
