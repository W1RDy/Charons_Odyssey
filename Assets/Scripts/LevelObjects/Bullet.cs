using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(DirectionalMove))]
[RequireComponent(typeof(Collider2D))]
public class Bullet : MonoBehaviour, IPause
{
    [SerializeField] private float _speed;
    private float _damage;
    private int _layer;
    private CancellationToken _token;
    private PauseService _pauseService;
    private PauseTokenSource _pauseTokenSource;
    private PauseToken _pauseToken;

    public void Initialize(AttackableObjectIndex attackableObjectIndex, PauseService pauseService, float distance, float damage)
    {
        var attackedObj = attackableObjectIndex == AttackableObjectIndex.Enemy ? AttackableObjectIndex.Player : AttackableObjectIndex.Enemy;
        _layer = (int)attackedObj;

        var dirMove = GetComponent<DirectionalMove>();
        dirMove.SetSpeed(_speed);
        dirMove.Init(pauseService);

        _damage = damage;
        _token = this.GetCancellationTokenOnDestroy();

        _pauseService = pauseService;
        _pauseService.AddPauseObj(this);
        _pauseTokenSource = new PauseTokenSource();
        _pauseToken = _pauseTokenSource.Token;

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

    public void Pause()
    {
        _pauseTokenSource.Pause();
    }

    public void Unpause()
    {
        _pauseTokenSource.Unpause();
    }

    public void OnDestroy()
    {
        _pauseService.RemovePauseObj(this);
    }
}
