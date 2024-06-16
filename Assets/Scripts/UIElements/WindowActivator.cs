using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class WindowActivator : MonoBehaviour
{
    [SerializeField] private WindowService _windowService;
    [SerializeField] private GameObject _defaultMenuElements;
    [SerializeField] private GameObject _baseWindowElements;
    private PlayerController _controller;

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
        _window.ActivateWindow();

        if (_controller) _controller.IsControl = false;
        if (type == WindowType.MapWindow) _controller.IsInteractActivated = true;

        if (_defaultMenuElements != null && _defaultMenuElements.activeInHierarchy)
        {
            _defaultMenuElements.SetActive(false);
        }

        if (_baseWindowElements != null && !_baseWindowElements.activeInHierarchy && type != WindowType.LoseWindow && type != WindowType.MapWindow && type != WindowType.TradeWindow)
        {
            _baseWindowElements.SetActive(true);
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
        window.DeactivateWindow();

        if (_controller) _controller.IsControl = true;

        if (_defaultMenuElements != null && !_defaultMenuElements.activeInHierarchy)
        {
            _defaultMenuElements.SetActive(true);
        }

        if (_baseWindowElements != null && type == WindowType.PauseWindow)
        {
            _baseWindowElements.SetActive(false);
        }
    }

    public async void DeactivateWindowWithDelay(WindowType type, float delay)
    {
        var token = this.GetCancellationTokenOnDestroy();
        await Delayer.Delay(delay, token);
        if (!token.IsCancellationRequested) DeactivateWindow(type);
    }
}
