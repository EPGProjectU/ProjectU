using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using UnityEngine;

public class GameEventSystem : MonoBehaviour {
    public static GameEventSystem Instance;

    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public event Action<int> OnPlayerTakesDamage;
    public event Action OnPlayerDead;

    public void PlayerGetsDamage(int damage)
    {
        OnPlayerTakesDamage?.Invoke(damage);
    }

    public void PlayerIsDead()
    {
        OnPlayerDead?.Invoke();
    }
}
