using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BulletMove : MonoBehaviour, IMovable
{
    [SerializeField] private Vector3 _direction;
    private float _speed;

    private PauseToken _pauseToken;
    private Rigidbody2D _rb;

    public void Init(PauseToken pauseToken)
    {
        _pauseToken = pauseToken;
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!_pauseToken.IsCancellationRequested)
        {
            Move();
        }
    }

    public void Move()
    {
        _rb.velocity = transform.TransformDirection(_direction) * _speed;
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }
}