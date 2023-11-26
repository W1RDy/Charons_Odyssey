using System.Collections;
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

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        _movable = GetComponent<IMovableWithStops>();
        _movable.SetSpeed(_speed);
        _trigger.TriggerWorked += _movable.StartMove;
        _trigger.TriggerTurnedOff += _movable.StopMove;
    }

    private void Update()
    {
        if (_target && Vector3.Distance(_target.position, transform.position) < _hitDistance)
        {
            _movable.StopMove();
            Attack();
        }
        else if (_trigger.playerInTrigger && State != EnemyStates.Moving && Vector3.Distance(_target.position, transform.position) > _hitDistance)
        {
            _movable.StartMove();
        }
    }

    public override void Attack()
    {
        if (State == EnemyStates.Idle)
        {
            ChangeState(EnemyStates.Attacking);
            var player = FinderHittableObjects.FindHittableObjectByCircle(_hitDistance, transform.position, AttackableObjectIndex.Enemy)[0];
            if (player != null) player.TakeHit(_damage);
            WaitCooldown();
        }
    }

    private void WaitCooldown()
    {
        ChangeState(EnemyStates.WaitingCooldown);
        StartCoroutine(Delayer.DelayCoroutine(_cooldown, () => ChangeState(EnemyStates.Idle)));
    }

    public void GetRecline(Transform _recliner, float _reclineForce)
    {
        if (_hp > 0)
        {
            Debug.Log("Recline");
            _movable.StopMove();
            var _previosState = State;
            ChangeState(EnemyStates.Reclined);
            _rb.AddForce((transform.position - _recliner.position).normalized * _reclineForce, ForceMode2D.Impulse);
            StartCoroutine(Delayer.DelayCoroutine(1f, () => ChangeState(_previosState)));
        }
    }

    private void OnDestroy()
    {
        _trigger.TriggerWorked -= _movable.StartMove;
        _trigger.TriggerTurnedOff -= _movable.StopMove;
    }
}
