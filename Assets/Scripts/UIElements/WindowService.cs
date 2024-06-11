using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WindowService : MonoBehaviour
{
    [SerializeField] private Window[] _windows;
    private Dictionary<WindowType, Window> _windowsDictionary;

    private void Awake()
    {
        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        _windowsDictionary = new Dictionary<WindowType, Window>();
        foreach (var window in _windows) _windowsDictionary[window.Type] = window; 
    }

    public Window GetWindow(WindowType type) => _windowsDictionary[type];
}

public enum WindowType
{
    LoseWindow,
    TradeWindow,
    PauseWindow,
    MenuWindow,
    SettingsWindow,
    ResetDataWindow,
    MapWindow,
    ControllingWindow,
}
