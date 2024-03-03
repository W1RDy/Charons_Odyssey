using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class StaminaIndicator : MonoBehaviour
{
    [SerializeField] private float _offsetDuration;
    [SerializeField] private float _offsetSpeed;
    private float _maxStaminaValue;

    [SerializeField] private Image _fillingImage;
    [SerializeField] private Image _fillingImageOffset;

    private CancellationToken _destroyToken;
    private CancellationTokenSource _tokenSource;
    private Action CurrentChanger;
    private Action<float> OffsetCallback;

    public void Initialize(float maxStaminaValue)
    {
        _maxStaminaValue = maxStaminaValue;
        _tokenSource = new CancellationTokenSource();
        _destroyToken = this.GetCancellationTokenOnDestroy();
        OffsetCallback = value => _fillingImageOffset.fillAmount = value;
    }

    public void SetStamine(float value)
    {
        _fillingImage.fillAmount = value / _maxStaminaValue;

        if (_fillingImageOffset.fillAmount < _fillingImage.fillAmount)
        {
            _tokenSource.Cancel();
            _tokenSource = new CancellationTokenSource();
            _fillingImageOffset.fillAmount = _fillingImage.fillAmount;
        }
        else if (CurrentChanger == null)
        {
            CurrentChanger = ChangeOffsetImage;
            CurrentChanger?.Invoke();
        }
    }

    public async void ChangeOffsetImage()
    {
        await Delayer.Delay(_offsetDuration, _destroyToken);
        if (_destroyToken.IsCancellationRequested) return;

        var token = CancellationTokenSource.CreateLinkedTokenSource(_destroyToken, _tokenSource.Token).Token;

        while (_fillingImage.fillAmount < _fillingImageOffset.fillAmount)
        {
            var imageValue = _fillingImage.fillAmount;
            var offsetValue = _fillingImageOffset.fillAmount;
            await SmoothChanger.SmoothChange(offsetValue, imageValue, (offsetValue - imageValue) / _offsetSpeed, OffsetCallback, token);
            if (token.IsCancellationRequested) break;
        }
        CurrentChanger = null;
    }
}