using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Move State", menuName = "Player's State/Move State")]
public class PlayerMoveState : PlayerState
{
    protected PlayerMove _playerMove;
    private float _horizontalMoveValue = 0;
    private PlayerController _controller;

    public override void Initialize(Player player, PauseService pauseService, AudioMaster audioMaster)
    {
        base.Initialize(player, pauseService, audioMaster);

        _playerMove = player.GetComponent<PlayerMove>();
        _controller = player.GetComponent<PlayerController>();
    }

    public override void Enter()
    {
        base.Enter();
        _player.SetAnimation("Move", true);
        _audioMaster.PlaySound("Walk");
    }

    public override void Update()
    {
        if (!_isPaused)
        {
            if (Input.GetAxis("Horizontal") != _horizontalMoveValue)
            {
                _horizontalMoveValue = Input.GetAxis("Horizontal");

                if (_horizontalMoveValue == 0 || _controller.IsControl == false)
                {
                    IsStateFinished = true;
                    return;
                }
                else IsStateFinished = false;
                _playerMove.SetDirection(new Vector2(_horizontalMoveValue, 0));
            }
        }
        else if (_horizontalMoveValue != float.MinValue) _horizontalMoveValue = float.MinValue;
    }

    public override void Exit()
    {
        base.Exit();
        _playerMove.SetDirection(Vector2.zero);
        _player.SetAnimation("Move", false);
        _audioMaster.StopSound("Walk");
    }

    public override void ResetValues()
    {
        _horizontalMoveValue = 0;
    }
}
