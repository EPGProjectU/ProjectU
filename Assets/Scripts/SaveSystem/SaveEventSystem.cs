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

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void RuntimeInit()
    {
        var go = new GameObject { name = "SaveEventSystem" };
        Instance =  go.AddComponent<SaveEventSystem>();
        DontDestroyOnLoad(go);
    }

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

        string tmpName = (string)data.loadedScenes.ToArray().GetValue(0);

        SceneManager.LoadScene(tmpName);

        data.loadedScenes.Remove(tmpName);

        foreach (string sceneName in data.loadedScenes)
        {
            if (sceneName != "SaveSystemScene")SceneManager.LoadScene(sceneName,LoadSceneMode.Additive);
        }

        StartCoroutine(OnSceneLoaded());
    }

    private IEnumerator OnSceneLoaded()
    {
        yield return new WaitForSeconds(0.1f);
        OnLoadData?.Invoke(data);
    }
}
