using UnityEngine;
using Zenject;

public class PlayerStaminaHandler
{
    private float _maxStaminaValue;
    private StaminaIndicator _staminaIndicator;

    public PlayerStaminaHandler(float maxStaminaValue, StaminaIndicator staminaIndicator)
    {
        _staminaIndicator = staminaIndicator;
        _maxStaminaValue = maxStaminaValue;
        _staminaIndicator.Initialize(maxStaminaValue);
    }

    public void UseStamina(float value, ref float currentStaminaValue)
    {
        currentStaminaValue = Mathf.Clamp(currentStaminaValue - value, 0, _maxStaminaValue);
        _staminaIndicator.SetStamine(currentStaminaValue);
    }

    public void ChangeStaminaTo(float value, ref float currentStaminaValue)
    {
        currentStaminaValue = Mathf.Clamp(value, 0, _maxStaminaValue);
        _staminaIndicator.SetStamine(currentStaminaValue);
    }

    public void RefillStamina(float value, ref float currentStaminaValue)
    {
        currentStaminaValue = Mathf.Clamp(currentStaminaValue + value, 0, _maxStaminaValue);
        _staminaIndicator.SetStamine(currentStaminaValue);
    }
}
