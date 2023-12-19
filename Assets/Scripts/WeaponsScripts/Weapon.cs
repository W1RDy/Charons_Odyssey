using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected WeaponData _weaponData;
    public Player player;
    public float Cooldown => _weaponData.Cooldown;
    public float Damage => _weaponData.Damage;
    public WeaponType GetWeaponType() => _weaponData.Type;
    public float Distance => _weaponData.Distance;
    protected bool _isCooldown;

    public virtual void Attack()
    {
        player.SetAttackState(this);
        WaitCooldown();
    }

    protected virtual void ApplyDamage(List<IHittable> _hittables)
    {
        foreach (var _hittable in _hittables)
        {
            var objWithHealth = _hittable as IHasHealth;
            if (objWithHealth != null) objWithHealth.TakeHit(Damage); 
        }
    }

    private async void WaitCooldown()
    {
        _isCooldown = true;
        var token = this.GetCancellationTokenOnDestroy();
        await Delayer.Delay(Cooldown, token);
        if (!token.IsCancellationRequested) _isCooldown = false;
    }

    public virtual void FinishAttack()
    {
        Debug.Log("Finish");
    }
}

public abstract class Guns : Weapon
{
    public float PatronsCount => (_weaponData as GunsData).PatronsCount;
    [SerializeField] protected Transform shootPoint;

    public override void Attack()
    {
        if (!_isCooldown)
        {
            base.Attack();
            player.weaponView.gameObject.SetActive(true);
            player.SetAttackAnimation(WeaponType.Pistol);
            Debug.Log("Gun");
            var _hittables = FinderObjects.FindHittableObjectByRay(Distance, player.weaponEnd.position, AttackableObjectIndex.Player);
            if (_hittables != null) ApplyDamage(_hittables);
        }
    }

    private void RotateGun()
    {
        shootPoint.position = CustomCamera.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition);
        var rotation = AngleService.GetAngleByTarget(player.weaponView, shootPoint);
        if ((rotation.eulerAngles.z > 180 && player.transform.localScale.x > 0) || (rotation.eulerAngles.z < 180 && player.transform.localScale.x < 0))
        {
            player.Flip();
        }
        player.weaponView.rotation = rotation;
    }

    public override void FinishAttack()
    {
        player.weaponView.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (player && player.weaponView.gameObject.activeInHierarchy) RotateGun();
    }
}

public abstract class ColdWeapon : Weapon
{
    public override void Attack()
    {
        if (!_isCooldown)
        {
            base.Attack();
            var _hittables = FinderObjects.FindHittableObjectByCircle(Distance, player.weaponPoint.position, AttackableObjectIndex.Player);
            if (_hittables != null) ApplyDamage(_hittables);
        }
    }
}
