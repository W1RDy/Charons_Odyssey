using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMove))]
public class Player : MonoBehaviour, IAttackableWithWeapon, IHasHealableHealth
{
    [SerializeField] private float _hp;
    [SerializeField] private float _speed;
    private PlayerMove _playerMove;

    private void Awake()
    {
        _playerMove = GetComponent<PlayerMove>();
        _playerMove.SetSpeed(_speed);
    }

    public void Attack(WeaponType weaponType)
    {
        WeaponManager.Instance.GetWeapon(weaponType).Attack();
    }

    public void TakeHeal(float healValue)
    {
        throw new NotImplementedException("TakeHeal is not implemented!");
    }

    public void TakeHit(float damage)
    {
        _hp -= damage;
        if (_hp <= 0) Death();
    }

    public void Death()
    {
        throw new NotImplementedException("You lost!");
    }
}
