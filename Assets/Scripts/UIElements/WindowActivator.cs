using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowActivator : MonoBehaviour
{
    [SerializeField] private WindowService _windowService;
    private PlayerController _controller;

    private void Awake()
    {
        _controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void ActivateWindow(WindowType _type)
    {
        var _window = _windowService.GetWindow(_type);
        _window.windowPrefab.SetActive(true);
        _controller.IsControl = false;
    }

    public async void ActivateWindowWithDelay(WindowType _type, float delay)
    {
        _controller.IsControl = false;
        Debug.Log("all");
        var token = this.GetCancellationTokenOnDestroy();
        await Delayer.Delay(delay, token);
        if (!token.IsCancellationRequested) ActivateWindow(_type); 
    }

    public void DeactivateWindow(WindowType _type)
    {
        var _window = _windowService.GetWindow(_type);
        _window.windowPrefab.SetActive(false);
        _controller.IsControl = true;
    }

    public async void DeactivateWindowWithDelay(WindowType _type, float delay)
    {
        _controller.IsControl = true;
        var token = this.GetCancellationTokenOnDestroy();
        await Delayer.Delay(delay, token);
        if (!token.IsCancellationRequested) DeactivateWindow(_type);
    }
}
