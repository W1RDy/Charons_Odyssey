using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Zenject;

[CreateAssetMenu(fileName = "Stay With Gun State", menuName = "Player's State/Stay With Gun State")]
public class PlayerStayWithGunState : PlayerStayState
{ 
    private Pistol _pistol;
    private Transform _shootPoint;
    private int _waitsMethodCount;
    private CustomCamera _camera;

    private PauseToken _pauseToken;
    private CancellationToken _token;

    public virtual void Initialize(Player player, Weapon weapon, IInstantiator instantiator, CustomCamera customCamera, AudioMaster audioMaster)
    {
        base.Initialize(player, instantiator, audioMaster);
        _camera = customCamera;
        _pistol = weapon as Pistol;
        try
        {
            _shootPoint = GameObject.Find("PlayerShootPoint").transform;
        }
        catch
        {
            _shootPoint = new GameObject("PlayerShootPoint").transform;
        }

        _pauseToken = _pauseHandler.GetPauseToken();
        _token = player.GetCancellationTokenOnDestroy();
    }

    public override void Enter()
    {
        base.Enter();
        _pistol.View.pistolView.gameObject.SetActive(true);
        IsStateFinished = false;
        _player.SetAnimation("HoldPistol", true);
        WaitSomeTime();
    }

    public override void Update()
    {
        if (!_isPaused)
        {
            base.Update();

            var rotation = AngleService.GetAngleByTarget(_pistol.View.pistolView, _shootPoint);
            var flipDirection = rotation.eulerAngles.z > 180 ? Vector2.left : Vector2.right;
            _player.Flip(flipDirection);
            RotateGun(rotation);
        }
    }

    private async void WaitSomeTime()
    {
        _waitsMethodCount++;
        var token = _player.GetCancellationTokenOnDestroy();
        await Delayer.DelayWithPause(4f, token, _pauseToken);
        _waitsMethodCount--;
        if (!token.IsCancellationRequested && _waitsMethodCount == 0)
        {
            IsStateFinished = true;
        }
    }

    private void RotateGun(Quaternion rotation)
    {
        _shootPoint.position = _camera.MainCamera.ScreenToWorldPoint(Input.mousePosition);

        _pistol.View.pistolView.rotation = rotation;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override async UniTask WaitTransition(PlayerStateType newStateType)
    {
        if (newStateType != PlayerStateType.AttackWithPistol)
        {
            _player.SetAnimation("HoldPistol", false);
            await UniTask.WaitWhile(() => _player.GetCurrentAnimationName().EndsWith("Shot"), cancellationToken: _token).SuppressCancellationThrow();

            if (_token.IsCancellationRequested) return;
            _pistol.View.pistolView.gameObject.SetActive(false);

            await UniTask.WaitWhile(() => _player.GetCurrentAnimationName().EndsWith("Pistol"), cancellationToken: _token).SuppressCancellationThrow();
        }
    }
}
