using JetBrains.Annotations;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Assertions;

public class QuestManagerEditorWindow : EditorWindow
{
    private ReorderableList questList;
    private bool questFoldout = true;

    private const string QuestsHeader = "Quests";

    [MenuItem("ProjectU/Progression/Quest Manager")]
    public static void ShowWindow()
    {
        GetWindow(typeof(QuestManagerEditorWindow));
    }

    private void OnEnable()
    {
        if (QuestManager.Data.database != null)
            questList = CreateQuestList(QuestManager.Data.database);
    }

    private ReorderableList CreateQuestList([NotNull] QuestDatabase database)
    {
        return new ReorderableList(database.quests, typeof(QuestGraph), true, true, true, true)
        {
            drawElementCallback = DrawQuestListItems,
            drawHeaderCallback = DrawQuestListHeader,
            onAddCallback = AddItemToQuestList,
            onRemoveCallback = RemoveItemFromQuestList,
            elementHeightCallback = (index) =>
            {
                Repaint();
                return EditorGUIUtility.singleLineHeight * 2f;
            }
        };
    }

    private void OnGUI()
    {
        EditorGUI.BeginChangeCheck();

        var questDatabase = EditorGUILayout.ObjectField(QuestManager.Data.database, typeof(QuestDatabase), false) as QuestDatabase;

        if (questDatabase)
        {
            if (questFoldout)
                questList?.DoLayoutList();
            else
                questFoldout = EditorGUILayout.Foldout(questFoldout, QuestsHeader);
        }

        if (!EditorGUI.EndChangeCheck())
            return;

        if (questDatabase != QuestManager.Data.database)
        {
            QuestManager.Data.database = questDatabase;

            if (questDatabase != null)
                questList = CreateQuestList(questDatabase);

            EditorUtility.SetDirty(QuestManager.Data);
        }
    }

    void DrawQuestListItems(Rect rect, int index, bool isActive, bool isFocused)
    {
        questList.elementHeight = EditorGUIUtility.singleLineHeight * (1 + index);
        var originalRectWidth = rect.width;
        var rectIndent = rect.x;
        rect.height = EditorGUIUtility.singleLineHeight;

        var quest = questList.list[index] as QuestGraph;

        //Assert.IsNotNull(quest);

        EditorGUI.LabelField(new Rect(rect.x, rect.y, 100, EditorGUIUtility.singleLineHeight), "Name");

        quest.name = EditorGUI.TextField(rect, quest.name);

        rect.x += 50;
        rect.width -= 50;

        rect.y += rect.height;
        rect.x = rectIndent;
        rect.width = originalRectWidth;

        EditorGUI.ObjectField(rect, quest, typeof(QuestGraph), false);

        //element.name = EditorGUI.TextField(rect, element.name);
    }

    void DrawQuestListHeader(Rect rect)
    {
        //EditorGUI.LabelField(rect, "Quests");
        questFoldout = EditorGUI.Foldout(rect, questFoldout, QuestsHeader);
    }

    void AddItemToQuestList(ReorderableList list)
    {
        QuestManager.CreateQuest();

        EditorUtility.SetDirty(QuestManager.Data.database);
        AssetDatabase.SaveAssets();

        Repaint();
    }


    private void RemoveItemFromQuestList(ReorderableList list)
    {
        if (EditorUtility.DisplayDialog("Warning!",
                "Quest will be permanently deleted?", "Delete", "Cancel"))
        {
            var quest = list.list[list.index] as QuestGraph;
            QuestManager.RemoveQuest(quest);
            ReorderableList.defaultBehaviours.DoRemoveButton(list);

            EditorUtility.SetDirty(QuestManager.Data.database);
            AssetDatabase.SaveAssets();
        }

        Repaint();
    }
}