using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DirectionalMove : MonoBehaviour, IMovable
{
    [SerializeField] private Vector3 _direction;
    private float _speed;

    private PauseToken _pauseToken;

    public void Init(PauseToken pauseToken)
    {
        _pauseToken = pauseToken;
    }

    private void Update()
    {
        if (!_pauseToken.IsCancellationRequested)
        {
            Move();
        }
    }

    public void Move()
    {
        transform.Translate(_direction * _speed * Time.deltaTime);
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }
}
