using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Move State", menuName = "Player's State/Move State")]
public class PlayerMoveState : PlayerState
{
    protected PlayerMove _playerMove;
    private float _horizontalMoveValue = 0;
    private PlayerController _controller;

    public override void Initialize(Player player)
    {
        base.Initialize(player);

        _playerMove = player.GetComponent<PlayerMove>();
        _controller = player.GetComponent<PlayerController>();
    }

    public override void Enter()
    {
        _player.SetAnimation("Move", true);
    }

    public override void Update()
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

    public override void Exit()
    {
        base.Exit();
        _playerMove.SetDirection(Vector2.zero);
        _player.SetAnimation("Move", false);
    }

    public override void ResetValues()
    {
        _horizontalMoveValue = 0;
    }
}
