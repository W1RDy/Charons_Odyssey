using UnityEngine;
using Zenject;
using static UnityEngine.GraphicsBuffer;

[CreateAssetMenu(fileName = "Stay State", menuName = "Enemy's State/Stay State")]
public class EnemyIdleStayState : EnemyIdleState
{
    private Transform _target;

    public virtual void Initialize(Enemy _enemy, IInstantiator instantiator, Transform target)
    {
        base.Initialize(_enemy, instantiator);
        _target = target;
    }

    public override void Enter()
    {
        base.Enter();
        IsStateFinished = false;
    }

    public override void Update()
    {
        var vectorToTarget = (_target.transform.position - _enemy.transform.position);

        if (vectorToTarget.magnitude > _enemy.HitDistance)
        {
            IsStateFinished = true;

        }
        else
        {
            _enemy.Flip(vectorToTarget.normalized);
        }
    }
}
