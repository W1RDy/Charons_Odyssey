using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHittable
{
    public void TakeHit(float damage = 0);
}

public interface IHasHealth : IHittable
{
    public void Death();
}

public interface IHasHealableHealth : IHasHealth
{
    public void TakeHeal(float healValue);
}

public interface IReclinable : IHittable
{
    public void GetRecline(Transform _recliner, float _reclineForce);
}

public interface IHittableWithShield : IHittable
{
    public void TakeHit(float damage, Vector2 damageDirection);
}

public interface IParryingHittable: IHittable
{
    public bool IsReadyForParrying { get; set; }
    public void ApplyParrying();
}