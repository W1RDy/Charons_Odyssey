using UnityEngine;

public class Fist : ColdWeapon
{
    public override void Attack()
    {
        if (!_isCooldown)
        {
            player.SetAttackAnimation(WeaponType.Fist);
            Debug.Log("Fist");
        }
        base.Attack();
    }
}
