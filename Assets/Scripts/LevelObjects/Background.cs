using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Background : MonoBehaviour, IPause
{
    [SerializeField] private BackgroundLayer[] _layers;
    [Range(0, 2), SerializeField] private int _backgroundSpeed = 1;
    
    private CustomCamera _customCamera;

    private PauseService _pauseService;

    private bool _isPaused;
    private bool _isCanMoving;

    [Inject]
    private void Construct(CustomCamera customCamera, PauseService pauseService)
    {
        _customCamera = customCamera;
        _pauseService = pauseService;
        _pauseService.AddPauseObj(this);
    }

    private void Awake()
    {
        foreach (var layer in _layers)
        {
            var layerTransform = new GameObject(layer.layerIndex + "Layer").transform;
            layerTransform.SetParent(transform);
            layer.InitLayer(layerTransform, _customCamera);
            layer.speed *= _backgroundSpeed;
        }
    }

    public void ActivateMovement()
    {
        _isCanMoving = true;
    }

    public void DeactivateMovement()
    {
        _isCanMoving = false;
    }

    private void Update()
    {
        if (!_isPaused && _isCanMoving)
        {
            foreach (var layer in _layers) layer.Move();
        }
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
