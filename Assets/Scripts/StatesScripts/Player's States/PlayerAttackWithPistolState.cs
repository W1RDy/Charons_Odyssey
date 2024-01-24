using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "Attack With Pistol State", menuName = "Player's State/Attack With Pistol State")]
public class PlayerAttackWithPistolState : PlayerAttackBaseState
{
    private Pistol _pistol;
    private Transform _shootPoint;
    private Transform _pistolEnd;
    private CustomCamera _camera;

    public void Initialize(Player player, Weapon weapon, CustomCamera customCamera)
    {
        base.Initialize(player, weapon);
        _camera = customCamera;
        _pistol = _weapon as Pistol;
        _pistolEnd = _pistol.View.pistolEnd;
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
        _player.SetAnimation("PistolAttack", true);
        base.Enter();
    }

    public async override void Attack()
    {
        if (!IsCooldown && _pistol.PatronsCount > 0)
        {
            base.Attack();
            await UniTask.WaitUntil(() => _player.GetCurrentAnimationName().EndsWith("Shot"));
            _pistol.View.pistolView.gameObject.SetActive(true);
            Shot();
            _player.SetAnimation("HoldPistol", true);
        }
    }

    private void Shot()
    {
        var bullet = Instantiate(_pistol.BulletPrefab, _pistolEnd.position, _pistolEnd.rotation).GetComponent<Bullet>();
        if (_player.transform.localScale.x < 0) bullet.transform.eulerAngles = new Vector3(0, 0, bullet.transform.eulerAngles.z + 180);
        bullet.Initialize(AttackableObjectIndex.Player, _pistol.Distance, _pistol.Damage);
        _pistol.DisablePatrons(1);
    }

    public override void Update()
    {
        Debug.Log("AttackWithPistol_Update");
        base.Update();

        var rotation = AngleService.GetAngleByTarget(_pistol.View.pistolView, _shootPoint);
        if ((rotation.eulerAngles.z > 180 && _player.transform.localScale.x > 0) || (rotation.eulerAngles.z < 180 && _player.transform.localScale.x < 0))
        {
            _player.Flip();
        }
        RotateGun(rotation);
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
            await UniTask.WaitWhile(() => _player.GetCurrentAnimationName().EndsWith("Shot"));
            _pistol.View.pistolView.gameObject.SetActive(false);
            await UniTask.WaitWhile(() => _player.GetCurrentAnimationName().EndsWith("Pistol"));
        }
    }

    public override bool IsStateAvailable()
    {
        return !IsCooldown && _pistol.PatronsCount > 0;
    }
}
