using UnityEngine;
using Zenject;

public class PlayerStaminaController
{
    private float _maxStaminaValue;
    private StaminaIndicator _staminaIndicator;

    public PlayerStaminaController(float maxStaminaValue, StaminaIndicator staminaIndicator)
    {
        _staminaIndicator = staminaIndicator;
        _maxStaminaValue = maxStaminaValue;
        _staminaIndicator.Initialize(maxStaminaValue);
    }

    public void UseStamine(float value, ref float currentStaminaValue)
    {
        currentStaminaValue = Mathf.Clamp(currentStaminaValue - value, 0, _maxStaminaValue);
        _staminaIndicator.SetStamine(currentStaminaValue);
    }

    public void RefillStamine(float value, ref float currentStaminaValue)
    {
        currentStaminaValue = Mathf.Clamp(value, 0, _maxStaminaValue);
        _staminaIndicator.SetStamine(currentStaminaValue);
    }
}
