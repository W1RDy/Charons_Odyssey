using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BattleActivator : SubscribableClass, ISubscribable
{
    private List<Enemy> _enemies = new List<Enemy>();

    private GameStateController _gameStateController;

    private int _agressiveEnemiesCount;

    private Action EnemyBecomeAggressive;
    private Action EnemyBecomeCalm;

    [Inject]
    private void Construct(GameStateController gameStateController)
    {
        _gameStateController = gameStateController;

        EnemyBecomeAggressive = () =>
        {
            _agressiveEnemiesCount++;
            TryChangeState();
        };

        EnemyBecomeCalm = () =>
        {
            _agressiveEnemiesCount--;
            TryChangeState();
        };
    }

    public void AddEnemyToSubscribe(Enemy enemy)
    {
        enemy.OnEnemyAgressive += EnemyBecomeAggressive;
        enemy.OnEnemyCalm += EnemyBecomeCalm;

        _enemies.Add(enemy);
    }

    public void UnsubscribeToEnemy(Enemy enemy)
    {
        enemy.OnEnemyAgressive -= EnemyBecomeAggressive;
        enemy.OnEnemyCalm -= EnemyBecomeCalm;

        _enemies.Remove(enemy);
    }

    private void TryChangeState()
    {
        if (_agressiveEnemiesCount <= 0)
        {
            DeactivateBattle();
        }
        else
        {
            ActivateBattle();
        }
    }

    private void ActivateBattle()
    {
        _gameStateController.ActivateBattleState();
    }

    private void DeactivateBattle()
    {
        _gameStateController.ActivateResearchState();
    }

    public override void Subscribe() { }

    public override void Unsubscribe()
    {
        foreach (var enemy in _enemies)
        {
            enemy.OnEnemyAgressive -= EnemyBecomeAggressive;
            enemy.OnEnemyCalm -= EnemyBecomeCalm;
        }
    }
}
