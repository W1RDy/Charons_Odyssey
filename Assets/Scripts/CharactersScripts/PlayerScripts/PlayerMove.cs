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
    private Player _player;
    private bool _isClimb;
    private float _walkSpeed;
    private float _climbSpeed;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _player = GetComponent<Player>();
    }

    private void FixedUpdate()
    {
        if (_player.GetState() == State.Move || _player.GetState() == State.Climb) Move();
    }

    public void SetDirection(Vector3 direction)
    {
        if (!_isClimb)
        {
            if (direction != Vector3.zero)
            {
                _player.EnableState(State.Move);
            }
            else
            {
                _player.DisableState(State.Move);
            }

            _direction = direction;
        }
    }

    public void SetClimbDirection(Vector3 direction)
    {
        if (!_isClimb) _isClimb = true;
        _player.DisableState(State.Move);
        _player.EnableState(State.Climb);
        _direction = new Vector2(0, direction.y);
    }

    public void DisableMovement()
    {
        if (_rb.velocity != Vector2.zero)
        {
            _speed = _walkSpeed;
            SetDirection(Vector3.zero);
            _rb.velocity = Vector2.zero;
        }
    }

    public void FinishClimb()
    {
        if (_direction.y == 0)
        {
            _speed = _climbSpeed;
            _isClimb = false;
            _player.DisableState(State.Climb);
        }
    }

    public Vector3 GetDirection() => _direction;

    public void SetSpeed(float speed)
    {
        _speed = speed;
        _walkSpeed = _speed;
        _climbSpeed = _speed * 1.1f;
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

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && collision.GetContact(collision.contacts.Length - 1).point.y < transform.position.y) FinishClimb();
    }
}
