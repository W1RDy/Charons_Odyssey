using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMove _playerMove;
    private Player _player;
    private TimeCounter _timer;
    private bool _isControl = true;
    private Collider2D _playerCollider;
    public LadderMoveChecker LadderMoveChecker { get; private set; }
    public bool IsControl 
    {
        get => _isControl; 
        set 
        {
            if (typeof(bool) == value.GetType()) _isControl = value; 
        } 
    }

    private void Start()
    {
        _playerMove = GetComponent<PlayerMove>();
        _player = GetComponent<Player>();
        _timer = new TimeCounter();
        LadderMoveChecker = new LadderMoveChecker();
        _playerCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (_isControl)
        {
            if (Input.GetAxis("Horizontal") != 0 && !_playerMove.IsMoving())
            {
                _player.ChangeState(PlayerStateType.Move);
            }
            else if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0 && _player.StateMachine.CurrentState.IsStateFinished)
            {
                if (_player.StateMachine.CurrentState.GetStateType() == PlayerStateType.AttackWithPistol) _player.ChangeState(PlayerStateType.IdleWithGun);
                else _player.ChangeState(PlayerStateType.Idle);
            }

            if (LadderMoveChecker.IsCanMove(_player, Input.GetAxis("Vertical")) && _player.OnGround())
            {
                _player.ChangeState(PlayerStateType.Climb);
            }
            else if ((LadderMoveChecker.GetLadder() == null && Input.GetAxis("Vertical") != 0) || (LadderMoveChecker.GetLadder() != null && LadderMoveChecker.GetLadder().LadderIsUsing() && _player.OnGround())) // переделать
            {
                _player.ChangeState(PlayerStateType.Idle);
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
                var _interactable = FinderObjects.FindInteractableObjectByCircle(1f, _player.transform.position);
                if (_interactable != null) _interactable.Interact();
            }
            else if (Input.GetKeyDown(KeyCode.Q) && Inventory.Instance.HasItem(ItemType.FirstAidKit))
            {
                _player.ChangeState(PlayerStateType.Heal);
            }
        }
        else
        {
            _player.ChangeState(PlayerStateType.Idle);
            _playerMove.DisableMovement();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Ladder")
        {
            var ladder = collision.GetComponent<Ladder>();
            if (ladder.IsColliderOnLaddersCenter (_playerCollider)) LadderMoveChecker.SetLadderToCheck(ladder); 
            else LadderMoveChecker.RemoveLadderToCheck();
        }
    }
}
