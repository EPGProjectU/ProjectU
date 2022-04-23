using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class SaveEventSystem : MonoBehaviour
{
    public static SaveEventSystem Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    [SerializeField]
    private GameData data = new GameData();

    public event Action<GameData> OnLoadData;
    public event Action<GameData> OnSaveData;

    public void SaveData()
    {
        OnSaveData?.Invoke(data);

        XmlSerializer serializer = new XmlSerializer(typeof(GameData));
        FileStream stream = new FileStream(Application.dataPath + "/../Saves/save.xml", FileMode.Create);
        serializer.Serialize(stream, data);
        stream.Close();
    }

    public void LoadData()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(GameData));
        FileStream stream = new FileStream(Application.dataPath + "/../Saves/save.xml", FileMode.Open);

        GameData tmp = serializer.Deserialize(stream) as GameData;
        if (tmp != null)
        {
            data = tmp;
        }

        stream.Close();

        OnLoadData?.Invoke(data);
    }
}
