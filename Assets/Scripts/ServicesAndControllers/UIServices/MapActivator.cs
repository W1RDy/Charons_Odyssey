using Zenject;

public class MapActivator
{
    private WindowActivator _windowActivator;

    [Inject]
    private void Construct(WindowActivator windowActivator)
    {
        _windowActivator = windowActivator;
    }

    public void ActivateMap()
    {
        _windowActivator.ActivateWindow(WindowType.MapWindow);
    }

    public void DeactivateMap()
    {
        _windowActivator.DeactivateWindow(WindowType.MapWindow);
    }
}