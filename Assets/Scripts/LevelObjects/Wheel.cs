using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Wheel : MonoBehaviour, IPause
{
    [SerializeField] private bool _isRotating;
    private Animator _animator;
    private PauseService _pauseService;
    private bool _isPaused;

    [Inject]
    private void Construct(PauseService pauseService)
    {
        _pauseService = pauseService;
        _pauseService.AddPauseObj(this);
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        if (!_isRotating) DeactivateRotating();
    }

    public void ActivateRotating()
    {
        _animator.speed = 1;
        _isRotating = true;
    }

    public void DeactivateRotating()
    {
        _animator.speed = 0;
        _isRotating = false;
    }

    public void Pause()
    {
        if (_isRotating && !_isPaused)
        {
            _isPaused = true;
            _animator.speed = 0;
        }
    }

    public void Unpause()
    {
        if (_isRotating && _isPaused)
        {
            _isPaused = false;
            _animator.speed = 1;
        }
    }

    public void OnDestroy()
    {
        _pauseService.RemovePauseObj(this);
    }
}
