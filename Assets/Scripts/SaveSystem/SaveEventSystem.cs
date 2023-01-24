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
        SaveData("save");
    }

    public void SaveData(string name)
    {
        if (!Directory.Exists(Application.dataPath + "/../Saves")) Directory.CreateDirectory(Application.dataPath + "/../Saves");
        data.enemies = new List<EnemyData>();

        OnSaveData?.Invoke(data);

        int countLoaded = SceneManager.sceneCount;

        data.loadedScenes = new List<string>();

        for (int i = 0; i < countLoaded; i++)
        {
            data.loadedScenes.Add(SceneManager.GetSceneAt(i).name);
        }

        SaveItemsOnScene();
        SaveProgression();

        XmlSerializer serializer = new XmlSerializer(typeof(GameData));
        FileStream stream = new FileStream(Application.dataPath + "/../Saves/"+name+".xml", FileMode.Create);
        serializer.Serialize(stream, data);
        stream.Close();
    }

    private void SaveItemsOnScene()
    {
        var obs = FindObjectsOfType<GameObject>();
        List<string> tmp = new List<string>();
        foreach (GameObject go in obs)
        {
            if (go.GetComponent<ItemDisplay>()) tmp.Add(go.name);
        }
        data.itemsNamesOnScene = tmp;
    }

    private void SaveProgression()
    {
        var allTags = ProgressionManager.GetAllTags();
        data.activeTags = new List<string>();
        data.collectTags = new List<string>();

        foreach (TagNode tag in allTags)
        {
            if (tag.IsActive()) data.activeTags.Add(tag.Name);
            if (tag.IsCollected()) data.collectTags.Add(tag.Name);
        }
    }

    public void LoadData()
    {
        LoadData("save");
    }

    public void LoadData(string name)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(GameData));
        FileStream stream = new FileStream(Application.dataPath + "/../Saves/"+name+".xml", FileMode.Open);

        GameData tmp = serializer.Deserialize(stream) as GameData;
        if (tmp != null)
        {
            data = tmp;
        }

        stream.Close();

        string tmpName = (string)data.loadedScenes.ToArray().GetValue(0);
        
        SceneManager.sceneLoaded += OnSceneLoadedCorutine;

        SceneManager.LoadScene(tmpName);

        data.loadedScenes.Remove(tmpName);

        foreach (string sceneName in data.loadedScenes)
        {
            if (sceneName != "SaveSystemScene")SceneManager.LoadScene(sceneName,LoadSceneMode.Additive);
        }
    }

    private void OnSceneLoadedCorutine(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(OnSceneLoaded());
    }

    private IEnumerator OnSceneLoaded()
    {
        yield return new WaitForSeconds(0.1f);
        OnLoadData?.Invoke(data);
        SceneManager.sceneLoaded -= OnSceneLoadedCorutine;

        LoadItemsOnScene();
        LoadProgression();
    }

    private void LoadItemsOnScene()
    {
        var obs = FindObjectsOfType<GameObject>();
        List<string> tmpItems = new List<string>();
        foreach (GameObject go in obs)
        {
            if (go.GetComponent<ItemDisplay>() && !data.itemsNamesOnScene.Contains(go.name)) tmpItems.Add(go.name);
        }

        foreach (string itemName in tmpItems)
        {
            Destroy(GameObject.Find(itemName));
        }
    }

    private void LoadProgression()
    {
        foreach (string name in data.activeTags)
        {
            ProgressionManager.SetActiveTag(name, true);
        }

        foreach (string name in data.collectTags)
        {
            ProgressionManager.CollectTag(name, true);
        }
    }
}
