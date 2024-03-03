using System;
using System.Threading;
using UnityEngine;

public class StaminaController : IPause
{
    private const float StaminaRefillingSpeedPerSecond = 20f;
    private float _maxStaminaValue;
    private float _lastStaminaValue;

    private CancellationTokenSource _tokenSource;
    private CancellationToken _destroyToken;
    private IHasStamina _objWithStamina;

    private PauseTokenSource _pauseTokenSource;
    private PauseToken _pauseToken;
    private PauseService _pauseService;

    private Action<float> StaminaCallback;

    public StaminaController(float maxStaminaValue, CancellationToken destroyToken, IHasStamina objWithStamina, PauseService pauseService)
    {
        _tokenSource = new CancellationTokenSource();
        _destroyToken = destroyToken;

        _objWithStamina = objWithStamina;
        _maxStaminaValue = maxStaminaValue;

        _pauseService = pauseService;
        pauseService.AddPauseObj(this);
        _pauseTokenSource = new PauseTokenSource();
        _pauseToken = _pauseTokenSource.Token;

        StaminaCallback = staminaValue => _objWithStamina.ChangeStaminaTo(staminaValue);
    }

    public void StartRefillStamina()
    {
        StartControlStamina(true, StaminaRefillingSpeedPerSecond);
    }

    public void StartUsingStamina(float staminaUsingSpeedPerSeconds)
    {
        StartControlStamina(false, staminaUsingSpeedPerSeconds);
    }

    private void StartControlStamina(bool isRefill, float staminaSpeedPerSeconds)
    {
        StopControlStamina();
        _tokenSource = new CancellationTokenSource();
        if (isRefill) RefillStamina();
        else UseStamina(staminaSpeedPerSeconds);
    }

    public void StopRefillStamina()
    {
        StopControlStamina();
    }

    public void StopUsingStamina()
    {
        StopControlStamina();
    }

    private void StopControlStamina()
    {
        _tokenSource.Cancel();
    }

    private async void UseStamina(float staminaUsingSpeedPerSeconds)
    {
        while (_objWithStamina.GetStamina() > 0)
        {
            _lastStaminaValue = _objWithStamina.GetStamina();
            var token = CancellationTokenSource.CreateLinkedTokenSource(_destroyToken, _tokenSource.Token).Token;
            await SmoothChanger.SmoothChangeWithPause(_lastStaminaValue, _lastStaminaValue - staminaUsingSpeedPerSeconds, 1f, StaminaCallback, token, _pauseToken);
            if (token.IsCancellationRequested) break;
        }
    }

    private async void RefillStamina()
    {
        while (_objWithStamina.GetStamina() < _maxStaminaValue)
        {
            _lastStaminaValue = _objWithStamina.GetStamina();
            var token = CancellationTokenSource.CreateLinkedTokenSource(_destroyToken, _tokenSource.Token).Token;
            await SmoothChanger.SmoothChangeWithPause(_lastStaminaValue, _lastStaminaValue + StaminaRefillingSpeedPerSecond, 1f, StaminaCallback, token, _pauseToken);
            if (token.IsCancellationRequested) break;
        }
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