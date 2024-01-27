using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemyFactory : IEnemyFactory
{
    private EnemyService _enemyService;
    private ICustomInstantiator _instantiator;

    public EnemyFactory(EnemyService enemyService, ICustomInstantiator instantiator)
    {
        _enemyService = enemyService;
        _instantiator = instantiator;
    }

    public Enemy Create(EnemyType enemyType, Direction direction, bool isAvailable, Vector2 position)
    {
        Debug.Log("SpawnEmemies");
        var enemyObj = _enemyService.GetEnemyPrefab(enemyType).GetComponent<Enemy>();
        var enemy = _instantiator.InstantiatePrefabForComponent<Enemy>(enemyObj, position);
        enemy.InitializeEnemy(direction, isAvailable);
        return enemy;
    }
}
