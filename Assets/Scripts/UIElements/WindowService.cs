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
        foreach (var _window in _windows) _windowsDictionary[_window.type] = _window; 
    }

    public WindowConfig GetWindow(WindowType _type) => _windowsDictionary[_type];
}

[Serializable]
public class WindowConfig
{
    public WindowType type;
    public GameObject windowPrefab;
}

public enum WindowType
{
    LoseWindow
}
