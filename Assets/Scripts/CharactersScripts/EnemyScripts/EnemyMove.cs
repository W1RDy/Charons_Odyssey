using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour, IMovableWithFlips, IMovableWithStops
{
    [SerializeField] private Transform _target;
    private Enemy _enemy;
    private float _speed;
    private bool _isMove;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        if (_isMove && _target) Move();
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
        if (StatesManager.Instance.IsCanMakeTransition(tag, _enemy.State, State.Move))
        {
            _isMove = true;
            _enemy.EnableState(State.Move);
        }
    }

    public void StopMove()
    {
        if (_enemy.State == State.Move)
        {
            _isMove = false;
            _rb.velocity = Vector2.zero;
            _enemy.DisableState(State.Move);
        }
    }
}
