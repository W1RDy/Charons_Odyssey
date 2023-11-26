using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowActivator : MonoBehaviour
{
    [SerializeField] private WindowService _windowService;

    public void ActivateWindow(WindowType _type)
    {
        var _window = _windowService.GetWindow(_type);
        _window.windowPrefab.SetActive(true);
    }

    public void DeactivateWindow(WindowType _type)
    {
        var _window = _windowService.GetWindow(_type);
        _window.windowPrefab.SetActive(false);
    }
}
