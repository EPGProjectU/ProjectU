using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Very basic factions that are based on unity Tag(s) already assigned to each actor such as Troll, Goblin etc.. 
/// </summary>

[CreateAssetMenu(menuName = "ProjectU/AI/Faction Data")]
public class FactionData : ScriptableObject
{
    public List<string> enemyTags;
    public List<string> friendlyTags;
}
