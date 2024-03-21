using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class WindowActivator : MonoBehaviour, IPause
{
    [SerializeField] private WindowService _windowService;
    [SerializeField] private GameObject _defaultMenuElements;
    private PlayerController _controller;
    private PauseService _pauseService;
    private bool _isPaused;

    [Inject]
    private void Construct(PauseService pauseService)
    {
        _pauseService = pauseService;
        _pauseService.AddPauseObj(this);
    }

    private void Start()
    {
        try
        {
            _controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }
        catch { }
    }

    public void ActivateWindow(WindowType type)
    {
        var _window = _windowService.GetWindow(type);
        _window.windowPrefab.SetActive(true);

        if (_controller) _controller.IsControl = false;

        if (_defaultMenuElements != null && _defaultMenuElements.activeInHierarchy)
        {
            _defaultMenuElements.SetActive(false);
        }
    }

    public async void ActivateWindowWithDelay(WindowType type, float delay)
    {
        var token = this.GetCancellationTokenOnDestroy();
        await Delayer.Delay(delay, token);
        if (!token.IsCancellationRequested) ActivateWindow(type); 
    }

    public void DeactivateWindow(WindowType type)
    {
        var window = _windowService.GetWindow(type);
        window.windowPrefab.SetActive(false);

        if (_controller) _controller.IsControl = true;

        if (_defaultMenuElements != null && !_defaultMenuElements.activeInHierarchy)
        {
            _defaultMenuElements.SetActive(true);
        }
    }

    public async void DeactivateWindowWithDelay(WindowType type, float delay)
    {
        var token = this.GetCancellationTokenOnDestroy();
        await Delayer.Delay(delay, token);
        if (!token.IsCancellationRequested) DeactivateWindow(type);
    }

    public void Pause()
    {
        if (!_isPaused) _isPaused = true;
        ActivateWindow(WindowType.PauseWindow);
    }

    public void Unpause()
    {
        if (_isPaused) _isPaused = false;
        if (_defaultMenuElements == null) DeactivateWindow(WindowType.PauseWindow);
    }

    public void OnDestroy()
    {
        _pauseService.RemovePauseObj(this);
    }
}
