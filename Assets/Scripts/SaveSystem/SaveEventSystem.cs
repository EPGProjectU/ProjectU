using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        int countLoaded = SceneManager.sceneCount;

        data.loadedScenes = new List<string>();

        for (int i = 0; i < countLoaded; i++)
        {
            data.loadedScenes.Add(SceneManager.GetSceneAt(i).name);
        }

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

        int countLoaded = SceneManager.sceneCount;

        for (int i = 0; i < countLoaded; i++)
        {
            if(SceneManager.GetSceneAt(i).name!="SaveSystemScene")SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i).name);
        }

        foreach(string sceneName in data.loadedScenes)
        {
            if (sceneName != "SaveSystemScene")SceneManager.LoadScene(sceneName,LoadSceneMode.Additive);
        }

        OnLoadData?.Invoke(data);
    }
}
