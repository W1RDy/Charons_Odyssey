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
    private bool _isControl = true;
    public bool IsControl { set { if (typeof(bool) == value.GetType()) _isControl = value; } }

    private void Start()
    {
        _playerMove = GetComponent<PlayerMove>();
        _player = GetComponent<Player>();
        _timer = new TimeCounter();
    }

    private void Update()
    {
        if (_isControl)
        {
            if (Input.GetAxis("Horizontal") != _currentMoveValue.x || (Input.GetAxis("Horizontal") != 0 && _player.GetState() == State.Idle))
            {
                _currentMoveValue.x = Input.GetAxis("Horizontal");
                _playerMove.SetDirection(new Vector2(_currentMoveValue.x, 0));
            }

            if (Input.GetAxis("Vertical") != _currentMoveValue.y && _isCollideWithLadder)
            {
                _currentMoveValue.y = Input.GetAxis("Vertical");
                _playerMove.SetClimbDirection(_currentMoveValue);
            }
            else if (_currentMoveValue.y != 0 && !_isCollideWithLadder) // подправить
            {
                _currentMoveValue.y = 0;
                _playerMove.SetClimbDirection(_currentMoveValue);
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

            if (Input.GetKeyDown(KeyCode.E))
            {
                var _interactable = FinderObjects.FindInteractableObjectByCircle(2f, _player.transform.position);
                if (_interactable != null) _interactable.Interact();
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                _player.TakeHeal(3);
            }
        }
        else _playerMove.DisableMovement();
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
