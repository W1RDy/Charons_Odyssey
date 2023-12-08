using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 _currentMoveValue;
    private PlayerMove _playerMove;
    private Player _player;
    private TimeCounter _timer;
    private bool _isCollideWithLadder;

    private void Start()
    {
        _playerMove = GetComponent<PlayerMove>();
        _player = GetComponent<Player>();
        _timer = new TimeCounter();
    }

    private void Update()
    {
        if (Input.GetAxis("Horizontal") != _currentMoveValue.x)
        {
            _currentMoveValue.x = Input.GetAxis("Horizontal");
            _playerMove.SetDirection(new Vector2(_currentMoveValue.x, 0));
        }

        if (Input.GetAxis("Vertical") != _currentMoveValue.y && _isCollideWithLadder)
        {
            _currentMoveValue.y = Input.GetAxis("Vertical");
            _playerMove.SetDirection(new Vector2(0, _currentMoveValue.y));
        }
        else if (_currentMoveValue.y != 0 && !_isCollideWithLadder)
        {
            _currentMoveValue.y = 0;
            _playerMove.SetDirection(new Vector2(0, _currentMoveValue.y));
        }

        if (Input.GetKeyDown(KeyCode.Mouse0)) _player.Attack(WeaponType.Pistol);
        else if (Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyUp(KeyCode.Mouse1))
        {
            if (Input.GetKeyDown(KeyCode.Mouse1)) _timer.StartCounter();
            else
            {
                if (_timer.StopCounter() > 0.3f) _player.Attack(WeaponType.Paddle);
                else _player.Attack(WeaponType.Fist);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ladder") _isCollideWithLadder = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ladder") _isCollideWithLadder = false;
    }
}
