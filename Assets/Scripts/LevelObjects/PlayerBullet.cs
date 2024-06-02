using System.Collections.Generic;
using Zenject;

public class PlayerBullet : Bullet
{
    public override void Initialize(IInstantiator instantiator, float distance, float damage)
    {
        _layers = new List<int> { (int)AttackableObjectIndex.Enemy, (int)AttackableObjectIndex.EnemyBullet };
        base.Initialize(instantiator, distance, damage);
    }
}
