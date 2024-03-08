using System.Collections;
using System.Collections.Generic;

public interface IAttackable
{
    public void Attack();
}

public interface IAttackableWithWeapon
{
    public void Attack(WeaponType weaponType);
}

public enum AttackableObjectIndex
{
    Player = 7,
    Enemy = 6
}
