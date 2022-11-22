using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData 
{
    public List<string> loadedScenes = new List<string>();
    public List<string> itemsNamesOnScene = new List<string>();
    public PlayerData player = new PlayerData();
    public List<EnemyData> enemies = new List<EnemyData>();
}
