using UnityEngine;
using Zenject;

public class MapActivator
{
    private WindowActivator _windowActivator;
    public bool IsActivated { get; private set; }

    [Inject]
    private void Construct(WindowActivator windowActivator)
    {
        _windowActivator = windowActivator;
    }

    public void ActivateMap()
    {
        _windowActivator.ActivateWindow(WindowType.MapWindow);
        IsActivated = true;
    }

    public void DeactivateMap()
    {
        _windowActivator.DeactivateWindow(WindowType.MapWindow);
        IsActivated = false;
    }
}