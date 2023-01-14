using System.Collections;
using System.Collections.Generic;
using ProjectU.Core;
using UnityEngine;
using UnityEngine.InputSystem;

public class EquipmentSystem : MonoBehaviour
{
    public static EquipmentSystem Instance;

    public List<Item> Items = new List<Item>(); // new variable declared

    private void Awake()
    {
        // start of new code
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void setEqupiment(List<Item> items)
    {
        EquipmentSystem.Instance.Items = items;
    }

    [Awake]
    private static void Init()
    {
        var gameObject = new GameObject("Equipment System");
        DontDestroyOnLoad(gameObject);
        gameObject.AddComponent<EquipmentSystem>();
    }
}
