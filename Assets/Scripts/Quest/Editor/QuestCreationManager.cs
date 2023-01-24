using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using XNode;

public static class QuestCreationManager
{
    static QuestCreationManager()
    {
        TagNode.QuestCreationCallback += CreateQuest;
    }

    [DidReloadScripts]
    public static void Init() {}

    public static void CreateQuest(TagNode tagNode)
    {
        var template = Resources.Load<QuestGraph>("QuestTemplate");
        var quest = QuestManager.CreateQuest(template);
        quest.name = tagNode.Name;

        for (var i = 0; i < quest.nodes.Count; i++)
        {
            var node = quest.nodes[i];
            
            // Remove (Clone) from name
            node.name = template.nodes[i].name;

            if (node is TagHookNode tagHookNode)
                tagHookNode.tagHook.Tag = tagNode;
        }

        AssetDatabase.OpenAsset(quest);
    }
}