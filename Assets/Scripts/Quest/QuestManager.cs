using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

public class QuestManager
{
    public static QuestManagerData Data { get; private set; }

    private static readonly string ResourceDataPath = "Quest/QuestManagerData";

    public static void CreateQuest()
    {
        Assert.IsNotNull(Data, "Quest database is not set");

        var quest = ScriptableObject.CreateInstance<QuestGraph>();
        quest.name = "Quest";

#if UNITY_EDITOR
        AssetDatabase.AddObjectToAsset(quest, Data.database);
#endif
        Data.database.quests.Add(quest);
    }

    public static void RemoveQuest(QuestGraph quest)
    {
#if UNITY_EDITOR
        AssetDatabase.RemoveObjectFromAsset(quest);
#endif
    }

    /// <summary>
    /// Creates <see cref="QuestManagerData"/> file if it does not exist
    /// </summary>
    private static void CreateDataFile()
    {
#if UNITY_EDITOR
        var fullDataPath = $"Assets/Resources/{ResourceDataPath}.asset";

        Data = AssetDatabase.LoadAssetAtPath<QuestManagerData>(fullDataPath);

        if (Data)
            return;

        Debug.Log("QuestManagerData does not exist. Creating a new instance.");
        Directory.CreateDirectory(Path.GetDirectoryName(fullDataPath)!);
        Data = ScriptableObject.CreateInstance<QuestManagerData>();
        AssetDatabase.CreateAsset(Data, fullDataPath);

#else
        LoadData()
#endif
    }

    /// <summary>
    /// Loads <see cref="QuestManagerData"/> from <see cref="ResourceDataPath"/>
    /// </summary>
    private static void LoadData()
    {
        Data = Resources.Load<QuestManagerData>(ResourceDataPath);
    }

    static QuestManager() => CreateDataFile();
}