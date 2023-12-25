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
    private LadderMoveChecker _ladderMoveChecker;
    private CancellationToken _token;
    private bool _isCanFinishState;

    public override void Initialize(Player player)
    {
        base.Initialize(player);

        _playerMove = player.GetComponent<PlayerMove>();
        _ladderMoveChecker = _player.GetComponentInChildren<PlayerController>().LadderMoveChecker;
        _token = _player.GetCancellationTokenOnDestroy();
    }

    public override void Enter()
    {
        _player.SetAnimation("Climb", true);
        _ladder = _ladderMoveChecker.GetLadder();
        if (_ladder == null) IsStateFinished = true;
        else
        {
            _ladder.UseLadder();
            IsStateFinished = false;
            _isCanFinishState = false;
            WaitSomeTime();
        }
    }

    public override void Update()
    {
        if (_isCanFinishState && _player.OnGround())
        {
            IsStateFinished = true;
        }

        if (_ladderMoveChecker.IsCanMove(_player, Input.GetAxis("Vertical")))
            _verticalMoveValue = Input.GetAxis("Vertical");
        else _verticalMoveValue = 0;

        _playerMove.SetDirection(new Vector2(0, _verticalMoveValue));
    }

    private async void WaitSomeTime()
    {
        await Delayer.Delay(0.5f, _token);
        if (!_token.IsCancellationRequested) _isCanFinishState = true;
    }

    public override void Exit()
    {
        _ladder.ThrowLadder();
        _playerMove.SetDirection(Vector2.zero);
        _player.SetAnimation("Climb", false);
    }
}
