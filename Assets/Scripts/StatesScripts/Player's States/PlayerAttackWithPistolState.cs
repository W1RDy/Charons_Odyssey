using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack With Pistol State", menuName = "Player's State/Attack With Pistol State")]
public class PlayerAttackWithPistolState : PlayerAttackBaseState
{
    private Pistol _pistol;
    private Transform _shootPoint;
    private Transform _pistolEnd;

    public override void Initialize(Player player)
    {
        base.Initialize(player);
        _weapon = WeaponManager.Instance.GetWeapon(WeaponType.Pistol);
        _pistol = _weapon as Pistol;
        _pistolEnd = player.weaponEnd;
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
        if (!IsCooldown)
        {
            base.Attack();
            await UniTask.WaitUntil(() => _player.GetAnimationName().EndsWith("Shot"));
            _player.weaponView.gameObject.SetActive(true);
            var bullet = Instantiate(_pistol.BulletPrefab, _pistolEnd.position, _pistolEnd.rotation).GetComponent<Bullet>();
            if (_player.transform.localScale.x < 0) bullet.transform.eulerAngles = new Vector3(0, 0, bullet.transform.eulerAngles.z + 180); 
            bullet.Initialize(AttackableObjectIndex.Player, _pistol.Distance, _pistol.Damage);
            _player.SetAnimation("HoldPistol", true);
        }
    }

    public override void Update()
    {
        Debug.Log("AttackWithPistol_Update");
        base.Update();

        var rotation = AngleService.GetAngleByTarget(_player.weaponView, _shootPoint);
        if ((rotation.eulerAngles.z > 180 && _player.transform.localScale.x > 0) || (rotation.eulerAngles.z < 180 && _player.transform.localScale.x < 0))
        {
            _player.Flip();
        }
        RotateGun(rotation);
    }

    private void RotateGun(Quaternion rotation)
    {
        _shootPoint.position = CustomCamera.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition);

        _player.weaponView.rotation = rotation;
    }

    public override void Exit()
    {
        if (IsStateFinished)
        {
            base.Exit();
            _player.weaponView.gameObject.SetActive(false);
            _player.SetAnimation("PistolAttack", false);
        }
    }

    public override async UniTask WaitTransition(PlayerStateType newStateType)
    {
        _player.SetAnimation("HoldPistol", false);
        if (newStateType != PlayerStateType.IdleWithGun)
        {
            await UniTask.WaitWhile(() => _player.GetAnimationName().EndsWith("Shot"));
            _player.weaponView.gameObject.SetActive(false);
            await UniTask.WaitWhile(() => _player.GetAnimationName().EndsWith("Pistol"));
        }
    }
}
