using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalMove : MonoBehaviour, IMovable
{
    [SerializeField] private Vector3 _direction;
    private float _speed;

    private void Update()
    {
        Move();
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
