using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Goals : MonoBehaviour, IPause
{
    private Text _goals;
    private Action<float> _callback;
    private CancellationToken _token;
    private PauseService _pauseService;
    private PauseTokenSource _pauseTokenSource;
    private PauseToken _pauseToken;

    [Inject]
    private void Construct(PauseService pauseService)
    {
        _pauseService = pauseService;
        _pauseTokenSource = new PauseTokenSource();
        _pauseToken = _pauseTokenSource.Token;
        _pauseService.AddPauseObj(this);
    }

    private void Awake()
    {
        _goals = GetComponent<Text>();
        _callback = alphaValue => _goals.color = new Color(_goals.color.r, _goals.color.g, _goals.color.b, alphaValue);
        _token = this.GetCancellationTokenOnDestroy();
    }

    public async void ActivateGoal(string message)
    {
        _callback(0);
        _goals.text = message;
        SetGoals(message);
        await SmoothChanger.SmoothChangeWithPause(0, 1, 2f, _callback, _token, _pauseToken);
        if (_token.IsCancellationRequested) return;
        await Delayer.Delay(2f, _token);
        if (_token.IsCancellationRequested) return;
        await SmoothChanger.SmoothChangeWithPause(1, 0, 2f, _callback, _token, _pauseToken);
    }

    private void SetGoals(string goals)
    {
        _goals.text = goals;
    }

    public void Pause()
    {
        _pauseTokenSource.Pause();
    }

    public void Unpause()
    {
        _pauseTokenSource.Unpause();
    }

    public void OnDestroy()
    {
        _pauseService.RemovePauseObj(this);
    }
}