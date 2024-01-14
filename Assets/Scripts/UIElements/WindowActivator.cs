using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class WindowActivator : MonoBehaviour
{
    [SerializeField] private WindowService _windowService;
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
        _window.windowPrefab.SetActive(true);
        if (_controller) _controller.IsControl = false;
    }

    public async void ActivateWindowWithDelay(WindowType type, float delay)
    {
        if (_controller) _controller.IsControl = false;
        Debug.Log("all");
        var token = this.GetCancellationTokenOnDestroy();
        await Delayer.Delay(delay, token);
        if (!token.IsCancellationRequested) ActivateWindow(type); 
    }

    public void DeactivateWindow(WindowType type)
    {
        var window = _windowService.GetWindow(type);
        window.windowPrefab.SetActive(false);
        if (_controller) _controller.IsControl = true;
    }

    public async void DeactivateWindowWithDelay(WindowType type, float delay)
    {
        if (_controller) _controller.IsControl = true;
        var token = this.GetCancellationTokenOnDestroy();
        await Delayer.Delay(delay, token);
        if (!token.IsCancellationRequested) DeactivateWindow(type);
    }
}
