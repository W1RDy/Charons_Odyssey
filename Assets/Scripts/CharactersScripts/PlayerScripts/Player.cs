using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMove))]
public class Player : MonoBehaviour, IAttackableWithWeapon, IHasHealableHealth, IHasStates
{
    [SerializeField] private State _currentState;
    [SerializeField] private float _hp;
    [SerializeField] private float _speed;
    [SerializeField] private HpIndicator _hpIndicator;
    [SerializeField] private GameService _gameService;
    public Transform weaponPoint;
    private PlayerMove _playerMove;
    private Animator _animator;
    private List<State> _activatedStates;
    private float _maxHp;
    public Transform weaponView;
    public Transform weaponEnd;

    private void Awake()
    {
        _playerMove = GetComponent<PlayerMove>();
        _playerMove.SetSpeed(_speed);
        _animator = GetComponentInChildren<Animator>();
        _activatedStates = new List<State>();
        _maxHp = _hp;
    }

    public void Attack(WeaponType weaponType)
    {
        if (_currentState != State.Climb)
        {
            var weapon = WeaponManager.Instance.GetWeapon(weaponType);
            weapon.Attack();
        }
    }

    public void SetAttackState(Weapon weapon)
    {
        EnableState(State.Attack);
        WaitWhileAnimation(State.Attack, weapon);
    }

    private async void WaitWhileAnimation(State state, Weapon weapon = null)
    {
        var token = this.GetCancellationTokenOnDestroy();
        await UniTask.Yield();
        await Delayer.Delay(_animator.GetCurrentAnimatorStateInfo(0).length, token);
        if (state == State.Attack) weapon.FinishAttack();
        DisableState(state);
    }

    public void TakeHeal(float healValue)
    {
        _hp += healValue;
        if (_hp > _maxHp) _hp = _maxHp;
        _hpIndicator.SetHp(_hp);
        EnableState(State.Heal);
        WaitWhileAnimation(State.Heal);
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

    public void SetAttackAnimation(WeaponType weaponType)
    {
        _animator.SetInteger("AttackIndex", (int)weaponType + 1);
    }

    public void EnableState(State _state)
    {
        if (!_activatedStates.Contains(_state))
        {
            if (_state == State.Idle) _activatedStates.Clear();
            _activatedStates.Add(_state);
            _currentState = StatesManager.Instance.ChangeCurrentState(gameObject.tag, _currentState, _activatedStates);
            //Debug.Log("player current state = " + _currentState);
            SetAnimation(_state, true);
            if (_currentState == State.Attack || _currentState == State.Heal) _activatedStates.Remove(State.Move);
        }
    }

    public void DisableState(State _state)
    {
        if (_activatedStates.Contains(_state))
        {
            if (_state == State.Idle) throw new ArgumentException("Can't disable default state, set state instead!");
            _activatedStates.Remove(_state);
            _currentState = StatesManager.Instance.ChangeCurrentState(gameObject.tag, _currentState, _activatedStates);
            //Debug.Log("player current state = " + _currentState);
            SetAnimation(_state, false);
        }
    }

    public State GetState()
    {
        return _currentState;
    }

    public void SetIdleAnimation()
    {
        foreach (var parameter in _animator.parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Bool)
                _animator.SetBool(parameter.name, parameter.defaultBool);
            else if (parameter.type == AnimatorControllerParameterType.Int)
                _animator.SetInteger(parameter.name, parameter.defaultInt);
        }
    }

    public void SetAnimation(State state, bool _isActivateState)
    {
        if (state == State.Idle) SetIdleAnimation();
        else if (state == State.Attack && !_isActivateState) _animator.SetInteger("AttackIndex", 0);
        else
        {
            try { _animator.SetBool(state.ToString(), _isActivateState); }
            catch { if (_isActivateState) _animator.SetTrigger(state.ToString()); }
        }
    }

    public void Flip() => _playerMove.Flip();
}
