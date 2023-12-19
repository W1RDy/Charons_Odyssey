using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Threading;
using UnityEngine;

public class EnemyDefault : Enemy, IReclinable
{
    [SerializeField] private Trigger _trigger;
    [SerializeField] private float _speed;
    [SerializeField] private float _hitDistance;
    [SerializeField] private float _damage;
    [SerializeField] private float _cooldown;
    private Transform _target;
    private IMovableWithStops _movable;
    private Rigidbody2D _rb;
    bool isReclined = false;
    CancellationToken token;

    protected override void Awake()
    {
        base.Awake();
        _rb = GetComponent<Rigidbody2D>();
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        _movable = GetComponent<IMovableWithStops>();
        _movable.SetSpeed(_speed);
        _trigger.TriggerWorked += StartMove;
        _trigger.TriggerTurnedOff += _movable.StopMove;
        token = this.GetCancellationTokenOnDestroy();
    }

    private void Update()
    {
        if (_isAvailable)
        {
            if (_target && Vector3.Distance(_target.position, transform.position) < _hitDistance)
            {
                _movable.StopMove();
                Attack();
            }
            else if (_trigger.playerInTrigger && State != State.Move && Vector3.Distance(_target.position, transform.position) > _hitDistance)
            {
                _movable.StartMove();
            }
        }
    }

    private void StartMove()
    {
        if (_isAvailable) _movable.StartMove();
    }

    public override async void Attack()
    {
        if (StatesManager.Instance.IsCanMakeTransition(tag, State, State.Attack))
        {
            EnableState(State.Attack);
            Debug.Log("Attack");
            var token = this.GetCancellationTokenOnDestroy();
            await Delayer.Delay(1, token);
            if (token.IsCancellationRequested) return;
            var player = FinderObjects.FindHittableObjectByCircle(_hitDistance, transform.position, AttackableObjectIndex.Enemy);
            if (player != null) player[0].TakeHit(_damage);

            await WaitCooldown();
        }
    }

    private async UniTask WaitCooldown()
    {
        try
        {
            Debug.Log("Wait");
            DisableState(State.Attack);
            EnableState(State.WaitCooldown);
            await Delayer.Delay(_cooldown, token);
            if (!isReclined && !token.IsCancellationRequested)
            {
                DisableState(State.WaitCooldown);
                EnableState(State.Idle);
            }
            else isReclined = false;
        }
        catch(OperationCanceledException) { }
    }

    public async void GetRecline(Transform _recliner, float _reclineForce)
    {
        if (_hp > 0)
        {
            Debug.Log("Recline");
            _movable.StopMove();
            EnableState(State.Recline);
            _rb.AddForce((transform.position - _recliner.position).normalized * _reclineForce, ForceMode2D.Impulse);
            await Delayer.Delay(1, token);
            if (!token.IsCancellationRequested)
            {
                isReclined = true;
                EnableState(State.Idle);
            }
        }
    }

    private void OnDestroy()
    {
        _trigger.TriggerWorked -= StartMove;
        _trigger.TriggerTurnedOff -= _movable.StopMove;
    }
}
