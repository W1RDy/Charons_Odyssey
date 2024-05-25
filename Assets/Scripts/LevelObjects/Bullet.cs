using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(DirectionalMove))]
[RequireComponent(typeof(Collider2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed;
    private float _damage;
    private int _layer;
    private CancellationToken _token;

    private PauseHandler _pauseHandler;
    private PauseToken _pauseToken;

    public void Initialize(AttackableObjectIndex attackableObjectIndex, IInstantiator instantiator, float distance, float damage)
    {
        var attackedObj = attackableObjectIndex == AttackableObjectIndex.Enemy ? AttackableObjectIndex.Player : AttackableObjectIndex.Enemy;
        _layer = (int)attackedObj;

        _pauseHandler = instantiator.Instantiate<PauseHandler>();
        _pauseToken = _pauseHandler.GetPauseToken();
        _token = this.GetCancellationTokenOnDestroy();

        var dirMove = GetComponent<DirectionalMove>();
        dirMove.SetSpeed(_speed);
        dirMove.Init(_pauseToken);

        _damage = damage;

        WaitUntilDestroy(distance);
    }

    public async void WaitUntilDestroy(float distance)
    {
        await Delayer.DelayWithPause(distance / _speed, _token, _pauseToken);
        if (!_token.IsCancellationRequested) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_layer == collision.gameObject.layer)
        {
            var hittable = collision.gameObject.GetComponent<IHittable>();
            hittable.TakeHit(_damage);
            if (hittable is IStunable stunable) stunable.ApplyStun();
            Destroy(gameObject);
        }
    }
}
