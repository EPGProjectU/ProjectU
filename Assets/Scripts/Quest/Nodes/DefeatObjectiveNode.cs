using UnityEngine;
using XNode;

public class DefeatObjectiveNode : Node, NotifyNodeInterface
{
    [Input]
    public NotifyNodeInterface.EmptyPort input;

    [Output]
    public NotifyNodeInterface.EmptyPort output;

    public int numberOfEnemies = 1;
    private int _defeatedEnemies;
    
    public override object GetValue(NodePort port) => null;
    
    public bool Notify(object payload)
    {
        var enemies = FindObjectsOfType<EnemyHealthSystem>();

        foreach (var enemy in enemies)
            enemy.deathCallback += EnemyDefeated;

        _defeatedEnemies = 0;
        return true;
    }

    // TODO proper implementation
    private void EnemyDefeated(HealthSystem obj)
    {
        if (++_defeatedEnemies < numberOfEnemies)
        {
            Debug.Log($"{graph.name}: objective defeat {_defeatedEnemies}/{numberOfEnemies} progress.");
            return;
        }

        Debug.Log($"{graph.name}: objective defeat {numberOfEnemies} completed.");
        
        NotifyNodeHelper.SendNotify(GetOutputPort(nameof(output)), null);

        var enemies = FindObjectsOfType<EnemyHealthSystem>();

        foreach (var enemy in enemies)
            enemy.deathCallback -= EnemyDefeated;
    }
}