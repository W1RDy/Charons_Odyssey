using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Cysharp.Threading.Tasks;
using Zenject;

public class Timer : MonoBehaviour, IPause
{
    private Text _indicator;
    private int _time;
    private bool _isWorked;
    private PauseService _pauseService;
    private PauseTokenSource _pauseTokenSource;
    private PauseToken _pauseToken;

    [Inject]
    private void Construct(PauseService pauseService)
    {
        _pauseService = pauseService;
        if (gameObject.activeInHierarchy)
        {
            _pauseService.AddPauseObj(this);
            _pauseTokenSource = new PauseTokenSource();
            _pauseToken = _pauseTokenSource.Token;
        }
    }

    private void Awake()
    {
        _indicator = GetComponent<Text>();
        StartTimer();
    }

    private void SetTimer()
    {
        _indicator.text = _time.ToString();
    }

    private async void TimerLife()
    {
        while (true)
        {
            var token = this.GetCancellationTokenOnDestroy();
            await Delayer.DelayWithPause(1, token, _pauseToken);
            if (token.IsCancellationRequested) StopTimer();

            if (!_isWorked) break;
            _time += 1;
            SetTimer();
        }
    }

    public void StopTimer()
    {
        _isWorked = false;
    }

    public void StartTimer()
    {
        _isWorked = true;
        TimerLife();
    }

    public void Pause()
    {
        _pauseTokenSource.Pause();
        StopTimer();
    }

    public void Unpause()
    {
        _pauseTokenSource.Unpause();
        StartTimer();
    }

    public void OnDestroy()
    {
        _pauseService.RemovePauseObj(this);
    }
}
