using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemyMove : MonoBehaviour, IMovableWithFlips, IMovableWithStops, IPause
{
    [SerializeField] private Transform _target;
    private float _speed;
    private bool _isMove;
    private Rigidbody2D _rb;
    private PauseService _pauseService;

    [Inject]
    private void Construct(Player player, PauseService pauseService)
    {
        _target = player.transform;
        _pauseService = pauseService;
        _pauseService.AddPauseObj(this);
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (_isMove && _target) Move();
    }

    public void Move()
    {
        var direction = -(transform.position - _target.position).normalized;
        _rb.velocity = direction * _speed;
        Flip(direction);
    }

    public void Move(Vector2 direction)
    {
        _rb.velocity = direction * _speed ;
        Flip(direction);
    }

    public void Flip(Vector2 direction)
    {
        if (direction.x > 0 && transform.localScale.x < 0 || direction.x < 0 && transform.localScale.x > 0)
            transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y);
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }


    public void StartMove()
    {
        _isMove = true;
    }

    public void StopMove()
    {
        _isMove = false;
        _rb.velocity = Vector2.zero;
    }

    public bool IsMoving()
    {
        return _isMove;
    }

    public void Pause()
    {
        StopMove();
    }

    public void Unpause()
    {
        
    }

    public void OnDestroy()
    {
        _pauseService.RemovePauseObj(this);
    }
}
