using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WindowService : MonoBehaviour
{
    [SerializeField] private WindowConfig[] _windows;
    private Dictionary<WindowType, WindowConfig> _windowsDictionary;

    private void Awake()
    {
        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        _windowsDictionary = new Dictionary<WindowType, WindowConfig>();
        foreach (var window in _windows) _windowsDictionary[window.type] = window; 
    }

    public WindowConfig GetWindow(WindowType type) => _windowsDictionary[type];
}

[Serializable]
public class WindowConfig
{
    public WindowType type;
    public GameObject windowPrefab;
}

public enum WindowType
{
    LoseWindow,
    TradeWindow,
    PauseWindow,
    MenuWindow,
    SettingsWindow
}
