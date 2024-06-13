using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class StaminaController : IPause
{
    private const float StaminaRefillingSpeedPerSecond = 33f;
    private float _maxStaminaValue;
    private float _lastStaminaValue;

    private CancellationTokenSource _staminaTokenSource;
    private CancellationToken _destroyToken;
    private IHasStamina _objWithStamina;

    private PauseTokenSource _pauseTokenSource;
    private PauseToken _pauseToken;
    private PauseService _pauseService;

    private Action<float> StaminaCallback;

    public StaminaController(float maxStaminaValue, CancellationToken destroyToken, IHasStamina objWithStamina, PauseService pauseService)
    {
        _staminaTokenSource = new CancellationTokenSource();
        _destroyToken = destroyToken;

        _objWithStamina = objWithStamina;
        _maxStaminaValue = maxStaminaValue;

        _pauseService = pauseService;
        pauseService.AddPauseObj(this);
        _pauseTokenSource = new PauseTokenSource();
        _pauseToken = _pauseTokenSource.Token;

        StaminaCallback = staminaValue => _objWithStamina.ChangeStaminaTo(staminaValue);
    }

    public async void ActivateRefillingStaminaCycle()
    {
        _staminaTokenSource = new CancellationTokenSource();
        var waitingToken = _staminaTokenSource.Token;

        await WaitUntilStartingRefillStamina(waitingToken);

        if (waitingToken.IsCancellationRequested) return;

        StartControlStamina(true, StaminaRefillingSpeedPerSecond);
    }

    public void StartUsingStamina(float staminaUsingSpeedPerSeconds)
    {
        StartControlStamina(false, staminaUsingSpeedPerSeconds);
    }

    private async UniTask WaitUntilStartingRefillStamina(CancellationToken waitingToken)
    {
        var token = CancellationTokenSource.CreateLinkedTokenSource(_destroyToken, waitingToken).Token;
        await Delayer.DelayWithPause(5, token, _pauseToken);
        if (waitingToken.IsCancellationRequested) Debug.Log("Interrupted");
    }

    private void StartControlStamina(bool isRefill, float staminaSpeedPerSeconds)
    {
        StopControlStamina();
        _staminaTokenSource = new CancellationTokenSource();
        if (isRefill) RefillStaminaCycle(_staminaTokenSource.Token);
        else UseStaminaCycle(staminaSpeedPerSeconds, _staminaTokenSource.Token);
    }

    public void StopRefillStamina()
    {
        StopControlStamina();
    }

    public void StopUsingStamina()
    {
        StopControlStamina();
        ActivateRefillingStaminaCycle();
    }

    private void StopControlStamina()
    {
        _staminaTokenSource.Cancel();
    }

    public void UseStamina(float value)
    {
        _objWithStamina.UseStamina(value);
    }

    private async void UseStaminaCycle(float staminaUsingSpeedPerSeconds, CancellationToken usingToken)
    {
        while (_objWithStamina.GetStamina() > 0)
        {
            _lastStaminaValue = _objWithStamina.GetStamina();
            var token = CancellationTokenSource.CreateLinkedTokenSource(_destroyToken, usingToken).Token;
            await SmoothChanger.SmoothChangeWithPause(_lastStaminaValue, _lastStaminaValue - staminaUsingSpeedPerSeconds, 1f, StaminaCallback, token, _pauseToken);
            if (token.IsCancellationRequested) break;
        }
    }

    private async void RefillStaminaCycle(CancellationToken refillingToken)
    {
        while (_objWithStamina.GetStamina() < _maxStaminaValue)
        {
            _lastStaminaValue = _objWithStamina.GetStamina();
            var token = CancellationTokenSource.CreateLinkedTokenSource(_destroyToken, refillingToken).Token;
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