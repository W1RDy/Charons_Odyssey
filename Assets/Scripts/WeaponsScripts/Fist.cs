using UnityEngine;

public class Fist : ColdWeapon
{
    public override void Attack()
    {
        if (!_isCooldown)
            Debug.Log("Fist");
        base.Attack();
    }
}
