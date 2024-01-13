using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour, IMovableWithFlips
{
    private Rigidbody2D _rb;
    private Vector3 _direction;
    private float _speed;
    private bool _isMove;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (_isMove) Move();
    }

    public void SetDirection(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            _isMove = true;
        }
        else
        {
            _rb.velocity = Vector2.zero;
            _isMove = false;
        }

        _direction = direction;
    }

    public void DisableMovement()
    {
        if (_rb.velocity != Vector2.zero)
        {
            SetDirection(Vector3.zero);
            _rb.velocity = Vector2.zero;
        }
    }

    public Vector3 GetDirection() => _direction;

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }

    public void Move()
    {
        _rb.velocity = _direction * _speed;
        if ((_direction.x > 0 && transform.localScale.x < 0) || (_direction.x < 0 && transform.localScale.x > 0)) Flip();
    }

    public void Flip()
    {
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
    }

    public bool IsMoving() => _isMove;
}
