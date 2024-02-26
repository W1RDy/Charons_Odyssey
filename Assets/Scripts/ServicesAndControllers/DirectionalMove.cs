using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DirectionalMove : MonoBehaviour, IMovable, IPause
{
    [SerializeField] private Vector3 _direction;
    private float _speed;
    private bool _isPaused;
    private PauseService _pauseService;

    [Inject]
    private void Construct(PauseService pauseService)
    {
        Init(pauseService);
    }

    public void Init(PauseService pauseService)
    {
        _pauseService = pauseService;
        _pauseService.AddPauseObj(this);
    }

    private void Update()
    {
        if (!_isPaused)
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

    public void Pause()
    {
        if (!_isPaused) _isPaused = true; 
    }

    public void Unpause()
    {
        if (_isPaused) _isPaused = false;
    }

    public void OnDestroy()
    {
        _pauseService.RemovePauseObj(this);
    }
}
