using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponData : ScriptableObject
{
    [SerializeField] private float _damage;
    [SerializeField] private float _cooldown;
    [SerializeField] private WeaponType _type;
    [SerializeField] private float _distance;

    public float Damage => _damage;
    public float Cooldown => _cooldown;
    public WeaponType Type => _type;
    public float Distance => _distance;
}

public abstract class ColdWeaponData : WeaponData
{

}

public abstract class GunsData : WeaponData
{
    [SerializeField] private int _patronsCount;
    [SerializeField] private Bullet _bullet;

    public float PatronsCount => _patronsCount;
    public Bullet Bullet => _bullet;
}
