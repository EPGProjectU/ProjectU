using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ProjectU/QuestDatabase", fileName = "QuestDatabase", order = 0)]
public class QuestDatabase : ScriptableObject
{
    [SerializeField]
    [HideInInspector]
    public List<QuestGraph> quests = new List<QuestGraph>();
}