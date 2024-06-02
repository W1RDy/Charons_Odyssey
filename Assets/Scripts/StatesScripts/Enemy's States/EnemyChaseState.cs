using UnityEngine;
using Zenject;

public abstract class EnemyChaseState : EnemyState
{
    protected Transform _target;

    public virtual void Initialize(Enemy enemy, IInstantiator instantiator, Transform target)
    {
        base.Initialize(enemy, instantiator);
        _target = target;
    }

    public override bool IsStateAvailable()
    {
        return base.IsStateAvailable();
    }
}