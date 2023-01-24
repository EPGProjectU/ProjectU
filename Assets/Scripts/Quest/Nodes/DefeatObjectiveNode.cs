using System.Collections.Generic;
using ProjectU;
using ProjectU.Core;
using UnityEngine;
using XNode;


[CreateNodeMenu("Quest/Objectives/Defeat")]
public class DefeatObjectiveNode : Node, NotifyNodeInterface
{
    [Input]
    public NotifyNodeInterface.EmptyPort input;
    
    public TagList tagList;

    [Output]
    public NotifyNodeInterface.EmptyPort output;

    public int numberOfEnemies = 1;
    private int _defeatedEnemies;

    public override object GetValue(NodePort port) => null;

    public bool Notify(object payload)
    {
        foreach (var tag in tagList)
        {
            if (!NotificationManger.DeathCallbacks.ContainsKey(tag))
                NotificationManger.DeathCallbacks[tag] = null;

            NotificationManger.DeathCallbacks[tag] -= EnemyDefeated;
            NotificationManger.DeathCallbacks[tag] += EnemyDefeated;
        }

        _defeatedEnemies = 0;
        return true;
    }


    // TODO proper implementation
    private void EnemyDefeated(object obj)
    {
        if (++_defeatedEnemies < numberOfEnemies)
        {
            Debug.Log($"{graph.name}: objective defeat {_defeatedEnemies}/{numberOfEnemies} progress.");
            return;
        }

        Debug.Log($"{graph.name}: objective defeat {numberOfEnemies} completed.");

        NotifyNodeHelper.SendNotify(GetOutputPort(nameof(output)), null);

        foreach (var tag in tagList)
        {
            NotificationManger.DeathCallbacks[tag] -= EnemyDefeated;
        }
    }
}