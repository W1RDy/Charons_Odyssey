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

    public void DeactivateWindow(WindowType _type)
    {
        var _window = _windowService.GetWindow(_type);
        _window.windowPrefab.SetActive(false);
        _controller.IsControl = true;
    }
}
