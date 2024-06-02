using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHittable
{
    public void TakeHit(HitInfo hitInfo);
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

public interface IParryingHittable : IHittable
{
    public bool IsReadyForParrying { get; set; }
    public void ApplyParrying();
}

public interface IStunable : IHittable
{
    public void ApplyStun();
}