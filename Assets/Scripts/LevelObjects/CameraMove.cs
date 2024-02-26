using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CameraMove : MonoBehaviour, IMovable
{
    [SerializeField] float _speed;
    private Transform _player;

    [Inject]
    private void Construct(Player player)
    {
        _player = player.transform;
    }

    private void Update()
    {
        Move();
    }

    public void Move()
    {
        if (_player != null)
        {
            var target = Vector2.MoveTowards(transform.position, new Vector2(_player.position.x, _player.position.y + 3.5f), _speed * Time.deltaTime);
            transform.position = new Vector3(target.x, target.y, -10);
        }
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }
}
