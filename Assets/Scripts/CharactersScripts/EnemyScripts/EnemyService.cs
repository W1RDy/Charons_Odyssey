using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyService : IService
{
    private const string SkeletonPath = "Enemies/Skeleton";
    private const string MinotaurPath = "Enemies/Minotaur";
    private const string GorgonPath = "Enemies/Gorgon";

    private GameObject _defaultEnemyPrefab;
    private GameObject _minotaurPrefab;
    private GameObject _gorgonPrefab;

    public void InitializeService()
    {
        LoadResources();
    }

    private void LoadResources()
    {
        _defaultEnemyPrefab = Resources.Load<Enemy>(SkeletonPath).gameObject;
        _minotaurPrefab = Resources.Load<Enemy>(MinotaurPath).gameObject;
        _gorgonPrefab = Resources.Load<Enemy>(GorgonPath).gameObject;
    }

    public GameObject GetEnemyPrefab(EnemyType enemyType)
    {
        switch (enemyType)
        {
            case EnemyType.Skeleton:
                return _defaultEnemyPrefab;
            case EnemyType.Minotaur:
                return _minotaurPrefab;
            case EnemyType.Gorgon:
                return _gorgonPrefab;
        }
        throw new System.InvalidOperationException("Enemy with type " + enemyType + " doesn't exist!");
    }
}