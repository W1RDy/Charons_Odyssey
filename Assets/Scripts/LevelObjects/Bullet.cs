using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(DirectionalMove))]
[RequireComponent(typeof(Collider2D))]
public abstract class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed;
    private float _damage;
    protected List<int> _layers;
    private CancellationToken _token;

    private PauseHandler _pauseHandler;
    private PauseToken _pauseToken;

    public virtual void Initialize(IInstantiator instantiator, float distance, float damage)
    {
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
        if (_layers.Contains(collision.gameObject.layer))
        {
            var hittable = collision.gameObject.GetComponent<IHittable>();
            hittable.TakeHit(new HitInfo(_damage, transform.InverseTransformDirection(Vector2.right), AdditiveHitEffect.Stun));
            Destroy(gameObject);
        }
    }
}