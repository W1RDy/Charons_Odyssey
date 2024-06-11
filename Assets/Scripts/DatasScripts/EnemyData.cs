using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyData : ScriptableObject
{
    [SerializeField] protected float _hp;
    [SerializeField] protected float _hitDistance;
    [SerializeField] protected float _hearNoiseDistance;

    [SerializeField] private float _attackCooldown;
    [SerializeField] private float _damage;

    [SerializeField] protected float _stunningTime;

    public float Hp => _hp;
    public float HitDistance => _hitDistance;
    public float HearNoiseDistance => _hearNoiseDistance;

    public float AttackCooldown => _attackCooldown;
    public float Damage => _damage;

    public float SunningTime => _stunningTime;
}

public abstract class MovableEnemyData : EnemyData
{
    [SerializeField] private float _speed;
    public float Speed => _speed;
}
