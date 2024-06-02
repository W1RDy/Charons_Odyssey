using System.Collections;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "Recline State", menuName = "Enemy's State/Recline State")]
public class EnemyReclineState : EnemyState
{
    protected Rigidbody2D _rb;
    protected Transform _recliner;
    protected float _reclineForce;

    public override void Initialize(Enemy enemy, IInstantiator instantiator)
    {
        base.Initialize(enemy, instantiator);
        _rb = enemy.GetComponent<Rigidbody2D>();
    }

    public void SetReclineParameters(Transform recliner, float reclineForce)
    {
        _recliner = recliner;
        _reclineForce = reclineForce;
    }

    public override void Enter()
    {
        base.Enter();
        IsStateFinished = false;
        GetRecline();
        IsStateFinished = true;
        _enemy.ChangeState(EnemyStateType.Stun);
    }

    private void GetRecline()
    {
        _rb.AddForce((_enemy.transform.position - _recliner.position).normalized * _reclineForce, ForceMode2D.Impulse);
    }
}
