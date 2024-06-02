using System.Collections.Generic;
using Zenject;

public class GorgonBullet : Bullet, IParryingHittable
{
    public bool IsReadyForParrying { get; set; }

    public override void Initialize(IInstantiator instantiator, float distance, float damage)
    {
        _layers = new List<int> { (int)AttackableObjectIndex.Player };
        IsReadyForParrying = true;
        base.Initialize(instantiator, distance, damage);
    }

    public void TakeHit(HitInfo hitInfo)
    {
        Destroy(gameObject);
    }

    public void ApplyParrying()
    {
        Destroy(gameObject);
    }
}