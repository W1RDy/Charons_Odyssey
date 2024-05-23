using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class PlayerStaminaHandler
{
    private float _maxStaminaValue;
    private float _currentStamina;

    private StaminaIndicator _staminaIndicator;

    private AudioMaster _audioMaster;
    private bool _isRepeatSound = true;

    public PlayerStaminaHandler(float maxStaminaValue, StaminaIndicator staminaIndicator, AudioMaster audioMaster)
    {
        _maxStaminaValue = maxStaminaValue;
        _currentStamina = maxStaminaValue;

        _staminaIndicator = staminaIndicator;
        _staminaIndicator.Initialize(maxStaminaValue);

        _audioMaster = audioMaster;
    }

    public void UseStamina(float value)
    {
        _currentStamina = Mathf.Clamp(_currentStamina - value, 0, _maxStaminaValue);
        _staminaIndicator.SetStamine(_currentStamina);
    }

    public void ChangeStaminaTo(float value)
    {
        _currentStamina = Mathf.Clamp(value, 0, _maxStaminaValue);
        _staminaIndicator.SetStamine(_currentStamina);
    }

    public void RefillStamina(float value)
    {
        _currentStamina = Mathf.Clamp(_currentStamina + value, 0, _maxStaminaValue);
        _staminaIndicator.SetStamine(_currentStamina);
    }

    public bool IsEnoughStamina(float neededStamina)
    {
        var isEnough = _currentStamina >= neededStamina;
        if (!isEnough && _isRepeatSound)
        {
            _audioMaster.PlaySound("StaminaIsOver");
            WaitBetweenSounds();
        }

        return isEnough;
    }

    public float GetStamina()
    {
        return _currentStamina;
    }

    private async void WaitBetweenSounds()
    {
        _isRepeatSound = false;
        await Delayer.Delay(2f, _staminaIndicator.GetCancellationTokenOnDestroy());
        _isRepeatSound = true;
    }
}
