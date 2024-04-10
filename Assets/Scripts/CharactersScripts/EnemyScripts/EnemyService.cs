using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyService : IService
{
    private const string DefaultEnemyPath = "Enemies/Enemy";
    private const string MinotaurPath = "Enemies/Minotaur";

    private GameObject _defaultEnemyPrefab;
    private GameObject _minotaurPrefab;

    public void InitializeService()
    {
        LoadResources();
    }

    private void LoadResources()
    {
        _defaultEnemyPrefab = Resources.Load<Enemy>(DefaultEnemyPath).gameObject;
        _minotaurPrefab = Resources.Load<Enemy>(MinotaurPath).gameObject;
    }

    public GameObject GetEnemyPrefab(EnemyType enemyType)
    {
        switch (enemyType)
        {
            case EnemyType.Default:
                return _defaultEnemyPrefab;
            case EnemyType.Minotaur:
                return _minotaurPrefab;
        }
        throw new System.InvalidOperationException("Enemy with type " + enemyType + " doesn't exist!");
    }
}