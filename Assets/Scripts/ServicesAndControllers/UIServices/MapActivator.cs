using UnityEngine;
using Zenject;

public class MapActivator
{
    private WindowActivator _windowActivator;
    public bool IsActivated { get; private set; }

    private PauseToken _pauseToken;

    [Inject]
    private void Construct(WindowActivator windowActivator, PauseService pauseService)
    {
        _windowActivator = windowActivator;

        var pauseHandler = new PauseHandler(pauseService);
        _pauseToken = pauseHandler.GetPauseToken();
    }

    public void ActivateMap()
    {
        if (!_pauseToken.IsCancellationRequested)
        {
            _windowActivator.ActivateWindow(WindowType.MapWindow);
            IsActivated = true;
        }
    }

    public void DeactivateMap()
    {
        if (!_pauseToken.IsCancellationRequested)
        {
            _windowActivator.DeactivateWindow(WindowType.MapWindow);
            IsActivated = false;
        }
    }
}