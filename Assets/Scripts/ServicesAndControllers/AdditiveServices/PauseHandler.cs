using System;
using UnityEngine;

public class PauseHandler : SubscribableClass, IPause
{
    private PauseService _pauseService;
    public PauseTokenSource _pauseTokenSource;

    private Action _pauseAction;
    private Action _unpauseAction;

    public PauseHandler(PauseService pauseService) : base()
    {
        Init(pauseService);
    }

    public PauseHandler(PauseService pauseService, Action pauseCallback, Action unpauseCallback) : base() 
    {
        Init(pauseService);

        _pauseAction = pauseCallback;
        _unpauseAction = unpauseCallback;
    }

    private void Init(PauseService pauseService)
    {
        _pauseService = pauseService;
        Subscribe();

        _pauseTokenSource = new PauseTokenSource();
    }

    public PauseToken GetPauseToken()
    {
        return _pauseTokenSource.Token;
    }

    public void Pause()
    {
        _pauseTokenSource.Pause();
        _pauseAction?.Invoke();
    }

    public void Unpause()
    {
        _pauseTokenSource.Unpause();
        _unpauseAction?.Invoke();
    }

    public override void Subscribe()
    {
        _pauseService.AddPauseObj(this);
    }

    public override void Unsubscribe()
    {
        Debug.Log("Unsubscribe");
        _pauseService.RemovePauseObj(this);
    }
}