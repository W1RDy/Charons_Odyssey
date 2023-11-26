using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMove))]
public class Player : MonoBehaviour, IAttackableWithWeapon, IHasHealableHealth
{
    [SerializeField] private float _hp;
    [SerializeField] private float _speed;
    [SerializeField] private HpIndicator _hpIndicator;
    [SerializeField] private GameService _gameService;
    public Transform weaponPoint;
    private PlayerMove _playerMove;
    private Animator _animator;

    private void Awake()
    {
        _playerMove = GetComponent<PlayerMove>();
        _playerMove.SetSpeed(_speed);
        _animator = GetComponentInChildren<Animator>();
    }

    public void Attack(WeaponType weaponType)
    {
        WeaponManager.Instance.GetWeapon(weaponType).Attack();
        _animator.SetTrigger("Hit");
    }

    public void SetMoveAnimation(bool isActivate)
    {
        _animator.SetBool("Move", isActivate);
    }

    public void TakeHeal(float healValue)
    {
        throw new NotImplementedException("TakeHeal is not implemented!");
    }

    public void TakeHit(float damage)
    {
        _hp -= damage;
        _hpIndicator.SetHp(_hp);
        if (_hp <= 0) Death();
    }

    public void Death()
    {
        Destroy(gameObject);
        _gameService.LoseGame();
    }
}
