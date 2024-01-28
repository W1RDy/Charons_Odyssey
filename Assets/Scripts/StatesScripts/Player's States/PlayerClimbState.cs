using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Climb State", menuName = "Player's State/Climb State")]
public class PlayerClimbState : PlayerState
{
    protected PlayerMove _playerMove;
    private float _verticalMoveValue = 0;
    private Ladder _ladder;
    private LadderUseChecker _ladderUseChecker;
    private PlayerColliderChecker _playerColliderChecker;

    public override void Initialize(Player player)
    {
        base.Initialize(player);

        _playerMove = player.GetComponent<PlayerMove>();
        _ladderUseChecker = _player.GetComponentInChildren<PlayerController>().LadderUseChecker;
        _playerColliderChecker = _player.GetComponent<PlayerColliderChecker>();
    }

    public override void Enter()
    {
        _player.SetAnimation("Climb", true);
        IsStateFinished = !_playerColliderChecker.TryGetLadder(out _ladder);

        if (_ladder)
        {
            _ladder.UseLadder();
        }
    }

    public override void Update()
    { 
        if (Input.GetAxis("Vertical") != _verticalMoveValue)
        {
            _verticalMoveValue = Input.GetAxis("Vertical");
            _playerMove.SetDirection(new Vector2(0, _verticalMoveValue));
        }

        if (!_playerColliderChecker.TryGetLadder(out var ladder) || _ladderUseChecker.IsCanThrow(_player, ladder, _verticalMoveValue))
        {
            IsStateFinished = true;
        }
    }

    public override void Exit()
    {
        _ladder.ThrowLadder();
        _playerMove.SetDirection(Vector2.zero);
        _player.SetAnimation("Climb", false);
    }
}
