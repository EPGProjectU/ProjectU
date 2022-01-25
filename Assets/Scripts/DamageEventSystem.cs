﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using UnityEngine;

public class DamageEventSystem : MonoBehaviour {
    public static DamageEventSystem Instance;

    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public event Action<int> OnPlayerTakesDamage;
    public event Action OnPlayerDealsDamage;
    public event Action OnPlayerDead;
    public event Action<int> OnEnemyTakesDamage;

    public void PlayerTakesDamage(int damage)
    {
        OnPlayerTakesDamage?.Invoke(damage);
    }

    public void PlayerDealsDamage()
    {
        OnPlayerDealsDamage?.Invoke();
    }

    public void PlayerIsDead()
    {
        OnPlayerDead?.Invoke();
    }

    public void EnemyTakesDamage(int damage)
    {
        OnEnemyTakesDamage?.Invoke(damage);
    }
}
