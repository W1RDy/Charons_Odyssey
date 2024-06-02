using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "Attack With Pistol State", menuName = "Player's State/Attack With Pistol State")]
public class PlayerAttackWithPistolState : PlayerAttackBaseState
{
    private Pistol _pistol;
    private Transform _shootPoint;
    private Transform _pistolEnd;

    private CustomCamera _camera;
    private Inventory _inventory;
    private NoiseEventHandler _noiseEventHandler;

    private IInstantiator _instantiator;

    private CancellationToken _token;

    public void Initialize(Player player, Weapon weapon, IInstantiator instantiator, Inventory inventory, CustomCamera customCamera, NoiseEventHandler noiseEventHandler, AudioMaster audioMaster)
    {
        base.Initialize(player, weapon, instantiator, audioMaster);
        _camera = customCamera;

        _pistol = _weapon as Pistol;
        _pistolEnd = _pistol.View.pistolEnd;

        _inventory = inventory;
        _instantiator = instantiator;

        _token = player.GetCancellationTokenOnDestroy(); 

        try
        {
            _shootPoint = GameObject.Find("ShootPoint").transform;
        }
        catch
        {
            _shootPoint = new GameObject("ShootPoint").transform;
        }

        _noiseEventHandler = noiseEventHandler;
    }

    public override void Enter()
    {
        _player.SetAnimation("PistolAttack", true);
        _audioMaster.PlaySound("Shot");
        base.Enter();
    }

    public async override void Attack()
    {
        if (!IsCooldown && _pistol.PatronsCount > 0)
        {
            base.Attack();
            await UniTask.WaitUntil(() => _player.GetCurrentAnimationName().EndsWith("Shot"), cancellationToken: _token).SuppressCancellationThrow();
            if (_token.IsCancellationRequested) return;

            _pistol.View.pistolView.gameObject.SetActive(true);
            Shot();
            _player.SetAnimation("HoldPistol", true);
        }
    }

    private void Shot()
    {
        var bullet = Instantiate(_pistol.BulletPrefab, _pistolEnd.position, _pistolEnd.rotation).GetComponent<PlayerBullet>();
        if (_player.transform.localScale.x < 0) bullet.transform.eulerAngles = new Vector3(0, 0, bullet.transform.eulerAngles.z + 180);
        bullet.Initialize(_instantiator, _pistol.Distance, _pistol.Damage);
        _inventory.RemoveItem(ItemType.Patrons, 1);
        _noiseEventHandler.MakeNoise(_pistolEnd.position);
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

    private void RotateGun(Quaternion rotation)
    {
        _shootPoint.position = _camera.MainCamera.ScreenToWorldPoint(Input.mousePosition);

        _pistol.View.pistolView.rotation = rotation;
    }

    public override void Exit()
    {
        if (IsStateFinished)
        {
            base.Exit();
            _pistol.View.pistolView.gameObject.SetActive(false);
            _player.SetAnimation("PistolAttack", false);
        }
    }

    public override async UniTask WaitTransition(PlayerStateType newStateType)
    {
        _player.SetAnimation("HoldPistol", false);
        if (newStateType != PlayerStateType.IdleWithGun)
        {
            await UniTask.WaitWhile(() => _player.GetCurrentAnimationName().EndsWith("Shot"), cancellationToken: _token);

            if (_token.IsCancellationRequested) return;
            _pistol.View.pistolView.gameObject.SetActive(false);

            await UniTask.WaitWhile(() => _player.GetCurrentAnimationName().EndsWith("Pistol"), cancellationToken: _token);
        }
    }

    public override bool IsStateAvailable()
    {
        return !IsCooldown && _pistol.PatronsCount > 0;
    }
}
