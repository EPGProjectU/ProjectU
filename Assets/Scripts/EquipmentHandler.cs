using ProjectU.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EquipmentHandler : MonoBehaviour
{
    // Start is called before the first frame update

    public static Item selectedItem;
    public Equipment equipment;
    public GameObject player;

    public Text itemUseSuccessMessage;
    
    void Start()
    {
        equipment = GameObject.Find("Player").GetComponent<Equipment>();
        player = GameObject.Find("Player").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Use()
    {
        selectedItem.Use(player);
        Debug.Log(selectedItem);
        equipment.items.Remove(selectedItem);
        SceneManager.UnloadSceneAsync((int)SceneEnum.EquipmentScen);
        SceneManager.LoadScene((int)SceneEnum.EquipmentScen, LoadSceneMode.Additive);

    }
}
