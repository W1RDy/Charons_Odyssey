using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

[CreateAssetMenu(fileName = "Stay With Gun State", menuName = "Player's State/Stay With Gun State")]
public class PlayerStayWithGunState : PlayerStayState
{ 
    private Pistol _pistol;
    private Transform _shootPoint;
    private int _waitsMethodCount;
    private CustomCamera _camera;

    public virtual void Initialize(Player player, Weapon weapon, CustomCamera customCamera)
    {
        base.Initialize(player);
        _camera = customCamera;
        _pistol = weapon as Pistol;
        try
        {
            _shootPoint = GameObject.Find("ShootPoint").transform;
        }
        catch
        {
            _shootPoint = new GameObject("ShootPoint").transform;
        }
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
        base.Update();

        var rotation = AngleService.GetAngleByTarget(_pistol.View.pistolView, _shootPoint);
        if ((rotation.eulerAngles.z > 180 && _player.transform.localScale.x > 0) || (rotation.eulerAngles.z < 180 && _player.transform.localScale.x < 0))
        {
            _player.Flip();
        }
        RotateGun(rotation);
    }

    private async void WaitSomeTime()
    {
        _waitsMethodCount++;
        var token = _player.GetCancellationTokenOnDestroy();
        await Delayer.Delay(4f, token);
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
            await UniTask.WaitWhile(() => _player.GetCurrentAnimationName().EndsWith("Shot"));
            _pistol.View.pistolView.gameObject.SetActive(false);
            await UniTask.WaitWhile(() => _player.GetCurrentAnimationName().EndsWith("Pistol"));
        }
    }
}
