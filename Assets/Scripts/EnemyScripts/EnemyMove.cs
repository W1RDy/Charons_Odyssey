using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour, IMovableWithFlips, IMovableWithStops
{
    [SerializeField] private Transform _target;
    private Enemy _enemy;
    private float _speed;
    private bool _isMove;
    private EnemyStates _previousState;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (_isMove) Move();
    }

    public void Move()
    {
        _rb.velocity = -(transform.position - _target.position).normalized * _speed;
        Flip();
    }

    public void Flip()
    {
        if (_target.position.x > transform.position.x && transform.localScale.x < 0 || _target.position.x < transform.position.x && transform.localScale.x > 0)
            transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y);
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }


    public void StartMove()
    {
        if (_enemy.State == EnemyStates.Idle || _enemy.State == EnemyStates.WaitingCooldown)
        {
            _isMove = true;
            _previousState = _enemy.State;
            _enemy.ChangeState(EnemyStates.Moving);
        }
    }

    public void StopMove()
    {
        if (_enemy.State == EnemyStates.Moving)
        {
            _isMove = false;
            _rb.velocity = Vector2.zero;
            _enemy.ChangeState(_previousState);
        }
    }
}
