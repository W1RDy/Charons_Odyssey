using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[CreateAssetMenu(fileName = "Climb State", menuName = "Player's State/Climb State")]
public class PlayerClimbState : PlayerState
{
    protected PlayerMove _playerMove;
    private float _verticalMoveValue = 0;
    private Ladder _ladder;
    private LadderUseChecker _ladderUseChecker;
    private PlayerColliderChecker _playerColliderChecker;
    private Rigidbody2D _rigidbody;
    private float _gravityScale;

    public override void Initialize(Player player, IInstantiator instantiator, AudioMaster audioMaster)
    {
        base.Initialize(player, instantiator, audioMaster);

        _playerMove = player.GetComponent<PlayerMove>();
        _rigidbody = player.GetComponent<Rigidbody2D>();
        _gravityScale = _rigidbody.gravityScale;
        _ladderUseChecker = _player.GetComponentInChildren<PlayerController>().LadderUseChecker;
        _playerColliderChecker = _player.GetComponent<PlayerColliderChecker>();
    }

    public override void Enter()
    {
        base.Enter();
        _player.SetAnimation("Climb", true);
        _audioMaster.PlaySound("Climb");

        IsStateFinished = !_playerColliderChecker.TryGetLadder(out _ladder);
        _rigidbody.gravityScale = 0;

        if (_ladder)
        {
            _ladder.UseLadder();
        }
    }

    public override void Update()
    { 
        if (!_isPaused)
        {
            if (Input.GetAxis("Vertical") != _verticalMoveValue)
            {
                _verticalMoveValue = Input.GetAxis("Vertical");
                if (_verticalMoveValue != 0) _player.SetAnimationSpeed(1);
                else _player.SetAnimationSpeed(0);
                _playerMove.SetDirection(new Vector2(0, _verticalMoveValue));
            }

            if (!_playerColliderChecker.TryGetLadder(out var ladder) || _ladderUseChecker.IsCanThrow(_player, ladder, _verticalMoveValue))
            {
                IsStateFinished = true;
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        _ladder.ThrowLadder();
        _playerMove.SetDirection(Vector2.zero);
        _player.SetAnimation("Climb", false);
        _audioMaster.StopSound("Climb");
    }

    public override void ResetValues()
    {
        base.ResetValues();
        _rigidbody.gravityScale = _gravityScale;
        _player.SetAnimationSpeed(1);
    }
}
