using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesCounter : SubscribableClass
{
    private List<Enemy> _enemies = new List<Enemy>();
    private int _enemiesCount;

    public event Action OnCounterUpdated;

    public void AddEnemy(Enemy enemy)
    {
        _enemiesCount++;
        _enemies.Add(enemy);
        enemy.OnEnemyDisable += DestroyEnemy;
    }

    private void DestroyEnemy()
    {
        _enemiesCount--;
        OnCounterUpdated?.Invoke();
    }

    public int GetEnemiesCount()
    {
        return _enemiesCount;
    }

    public override void Subscribe() { }

    public override void Unsubscribe()
    {
        foreach (Enemy enemy in _enemies)
        {
            enemy.OnEnemyDisable -= DestroyEnemy;
        }
    }
}