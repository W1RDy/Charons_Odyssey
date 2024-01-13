using Cysharp.Threading.Tasks;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Recline State", menuName = "Enemy's State/Recline State")]
public class EnemyReclineState : EnemyState
{
    protected Rigidbody2D _rb;
    protected Transform _recliner;
    protected float _reclineForce;

    public override void Initialize(Enemy enemy)
    {
        base.Initialize(enemy);
        _rb = enemy.GetComponent<Rigidbody2D>();
    }

    public void SetReclineParameters(Transform recliner, float reclineForce)
    {
        _recliner = recliner;
        _reclineForce = reclineForce;
    }

    public override void Enter()
    {
        IsStateFinished = false;
        GetRecline();
        WaitWhileRecline();
    }

    private void GetRecline()
    {
        _rb.AddForce((_enemy.transform.position - _recliner.position).normalized * _reclineForce, ForceMode2D.Impulse);
    }

    private async void WaitWhileRecline()
    {
        var token = _enemy.GetCancellationTokenOnDestroy();
        await Delayer.Delay(1, token);
        if (!token.IsCancellationRequested) IsStateFinished = true;
    }
}
