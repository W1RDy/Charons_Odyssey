using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(BulletMove))]
[RequireComponent(typeof(Collider2D))]
public abstract class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed;
    private float _damage;
    protected List<int> _layers;
    private CancellationToken _token;

    private PauseHandler _pauseHandler;
    private PauseToken _pauseToken;

    private LayerMask _layerMask;

    public virtual void Initialize(IInstantiator instantiator, float distance, float damage)
    {
        _pauseHandler = instantiator.Instantiate<PauseHandler>();
        _pauseToken = _pauseHandler.GetPauseToken();
        _token = this.GetCancellationTokenOnDestroy();

        var dirMove = GetComponent<BulletMove>();
        dirMove.SetSpeed(_speed);
        dirMove.Init(_pauseToken);

        _damage = damage;
        InitLayerMask();

        WaitUntilDestroy(distance);
    }

    private void InitLayerMask()
    {
        _layerMask = 1 << 3 | 1 << 13;
        foreach (var layer in _layers)
        {
            _layerMask |= 1 << layer;
        }
    }

    private void FixedUpdate()
    {
        Physics2D.queriesHitTriggers = false;
        var hit = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.right), 0.1f, _layerMask);
        Physics2D.queriesHitTriggers = true;

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Ground") || hit.collider.gameObject.layer == 13) Destroy(gameObject);
            else ApplyDamage(hit.collider);
        }
    }

    public async void WaitUntilDestroy(float distance)
    {
        await Delayer.DelayWithPause(distance / _speed, _token, _pauseToken);
        if (!_token.IsCancellationRequested) Destroy(gameObject);
    }

    private void ApplyDamage(Collider2D collider)
    {
        var hittable = collider.gameObject.GetComponent<IHittable>();
        hittable.TakeHit(new HitInfo(_damage, transform.InverseTransformDirection(Vector2.right), AdditiveHitEffect.Stun));
        Destroy(gameObject);
    }
}