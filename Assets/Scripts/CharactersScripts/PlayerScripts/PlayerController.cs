using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Zenject;

public class PlayerController : MonoBehaviour
{
    private PlayerMove _playerMove;
    private Player _player;
    private PlayerColliderChecker _playerColliderChecker;
    private bool _isControl = true;
    private IInputService _inputService;
    private Inventory _inventory;
    public LadderUseChecker LadderUseChecker { get; private set; }
    public bool IsControl 
    {
        get => _isControl; 
        set 
        {
            if (typeof(bool) == value.GetType()) _isControl = value; 
        } 
    }

    [Inject]
    private void Construct(Inventory inventory, IInputService inputService)
    {
        _inventory = inventory;
        _inputService = inputService;
    }

    private void Awake()
    {
        LadderUseChecker = new LadderUseChecker();
    }

    private void Start()
    {
        _playerMove = GetComponent<PlayerMove>();
        _player = GetComponent<Player>();
        _playerColliderChecker = GetComponent<PlayerColliderChecker>();
    }

    private void Update()
    {
        _inputService.UpdateInputs();
        if (_isControl)
        {
            if (_inputService.ButtonIsPushed(InputButtonType.Move) && !_playerMove.IsMoving())
            {
                _player.ChangeState(PlayerStateType.Move);
            }
            else if (!_inputService.ButtonIsPushed(InputButtonType.Move) && !_inputService.ButtonIsPushed(InputButtonType.Climb) && _player.StateMachine.CurrentState.IsStateFinished)
            {
                if (_player.StateMachine.CurrentState.GetStateType() == PlayerStateType.AttackWithPistol) _player.ChangeState(PlayerStateType.IdleWithGun);
                else _player.ChangeState(PlayerStateType.Idle);
            }

            var isCollideWithLadder = _playerColliderChecker.TryGetLadder(out var ladder);
            if (_inputService.ButtonIsPushed(InputButtonType.Climb) && LadderUseChecker.IsCanUse(_player, ladder, Input.GetAxis("Vertical")))
            {
                _player.ChangeState(PlayerStateType.Climb);
            }
            else if ((!isCollideWithLadder && _inputService.ButtonIsPushed(InputButtonType.Climb)) || LadderUseChecker.IsCanThrow(_player, ladder, Input.GetAxis("Vertical")))
            {
                Debug.Log("Exit");
                _player.ChangeState(PlayerStateType.Idle);
            }

            if (_inputService.ButtonIsPushed(InputButtonType.Shot)) _player.Attack(WeaponType.Pistol);
            else if (_inputService.ButtonIsPushed(InputButtonType.HeavyAttack)) _player.Attack(WeaponType.Paddle);
            else if (_inputService.ButtonIsPushed(InputButtonType.Attack)) _player.Attack(WeaponType.Fist);            

            if (_inputService.ButtonIsPushed(InputButtonType.Interact))
            {
                var interactable = FinderObjects.FindInteractableObjectByCircle(1f, _player.transform.position);
                interactable?.Interact();
            }
            else if (_inputService.ButtonIsPushed(InputButtonType.Heal) && _inventory.HasItem(ItemType.FirstAidKit))
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
}
