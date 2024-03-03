using System;
using System.Threading;
using UnityEngine;

public class StaminaRefiller : IPause
{
    private const float StaminaSpeedPerSecond = 20f;
    private float _maxStaminaValue;
    private float _lastStaminaValue;

    private CancellationTokenSource _tokenSource;
    private CancellationToken _destroyToken;
    private IHasStamina _objWithStamina;

    private PauseTokenSource _pauseTokenSource;
    private PauseToken _pauseToken;
    private PauseService _pauseService;
    private Action<float> ValueCallback;

    public StaminaRefiller(float maxStaminaValue, CancellationToken destroyToken, IHasStamina objWithStamina, PauseService pauseService)
    {
        _tokenSource = new CancellationTokenSource();
        _destroyToken = destroyToken;

        _objWithStamina = objWithStamina;
        _maxStaminaValue = maxStaminaValue;

        _pauseService = pauseService;
        pauseService.AddPauseObj(this);
        _pauseTokenSource = new PauseTokenSource();
        _pauseToken = _pauseTokenSource.Token;

        ValueCallback = staminaValue =>
        {
            Debug.Log(staminaValue);
            _objWithStamina.RefillStamina(staminaValue);
        };
    }


    public void StartRefillStamine()
    {
        _tokenSource = new CancellationTokenSource();
        RefillStamine();
    }

    public void StopRefillStamine()
    {
        _tokenSource.Cancel();
    }

    private async void RefillStamine()
    {
        while (_objWithStamina.GetStamina() < _maxStaminaValue)
        {
            _lastStaminaValue = _objWithStamina.GetStamina();
            var token = CancellationTokenSource.CreateLinkedTokenSource(_destroyToken, _tokenSource.Token).Token;
            Debug.Log("Refill");
            await SmoothChanger.SmoothChangeWithPause(_lastStaminaValue, _lastStaminaValue + StaminaSpeedPerSecond, 1f, ValueCallback, token, _pauseToken);
            if (token.IsCancellationRequested) break;
        }
        Debug.Log("Stop");
    }

    public void Pause()
    {
        _pauseTokenSource.Pause();
    }

    public void Unpause()
    {
        _pauseTokenSource.Unpause();
    }

    public void Unsubscribe()
    {
        _pauseService.RemovePauseObj(this);
    }
}