using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyService : IService
{
    private const string DefaultEnemy = "Enemy";

    private GameObject _defaultEnemyPrefab;

    public void InitializeService()
    {
        LoadResources();
    }

    private void LoadResources()
    {
        _defaultEnemyPrefab = Resources.Load<Enemy>("Enemies/" + DefaultEnemy).gameObject;
    }

    public GameObject GetEnemyPrefab(EnemyType enemyType)
    {
        switch (enemyType)
        {
            case EnemyType.Default:
                return _defaultEnemyPrefab;
        }
        throw new System.InvalidOperationException("Enemy with type " + enemyType + " doesn't exist!");
    }
}