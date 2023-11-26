using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour, IMovableWithFlips
{
    private Rigidbody2D _rb;
    private bool _isMove;
    private Vector3 _direction;
    private float _speed;
    private Player _player;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _player = GetComponent<Player>();
    }

    private void FixedUpdate()
    {
        _rb.velocity = _direction * _speed;
        if (_isMove) Move();
    }

    public void SetDirection(Vector3 direction)
    {
        if (direction != Vector3.zero) _isMove = true;
        else _isMove = false;
        _player.SetMoveAnimation(_isMove);

        if ((direction.x > 0 && transform.localScale.x < 0) || (direction.x < 0 && transform.localScale.x > 0))
            Flip();

        _direction = direction;
    }

    public Vector3 GetDirection() => _direction;

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }

    public void Move()
    {
        _rb.velocity = _direction * _speed;
    }

    public void Flip()
    {
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
    }
}
