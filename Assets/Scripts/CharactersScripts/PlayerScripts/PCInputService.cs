using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCInputService : IInputService, IService
{
    private Dictionary<InputButtonType, InputButton> _buttons;
    private TimeCounter _timeCounterForAttack;
    private TimeCounter _timeCounterForProtect;

    public void InitializeService()
    {
        _buttons = new Dictionary<InputButtonType, InputButton>();
        foreach (var buttonType in Enum.GetValues(typeof(InputButtonType)) as InputButtonType[])
        {
            _buttons.Add(buttonType, new InputButton(buttonType));
        }
        _timeCounterForAttack = new TimeCounter();
        _timeCounterForProtect = new TimeCounter();
    }

    public bool ButtonIsPushed(InputButtonType buttonType)
    {
        return _buttons[buttonType].IsPushed();
    }

    private void ActivateButton(InputButtonType buttonType)
    {
        ActivateDeactivateButton(buttonType, true);
    }

    private void DeactivateButton(InputButtonType buttonType)
    {
        ActivateDeactivateButton(buttonType, false);
    }

    private void ActivateDeactivateButton(InputButtonType buttonType, bool isActivate)
    {
        _buttons[buttonType].ChangeState(isActivate);
    }

    public void UpdateInputs()
    {
        if (Input.GetAxis("Horizontal") != 0)
        {
            ActivateButton(InputButtonType.Move);
        }
        else
        {
            DeactivateButton(InputButtonType.Move);
        }

        if (Input.GetAxis("Vertical") != 0)
        {
            ActivateButton(InputButtonType.Climb);
        }
        else
        {
            DeactivateButton(InputButtonType.Climb);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0)) ActivateButton(InputButtonType.Shot);
        else if (Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyUp(KeyCode.Mouse1))
        {
            if (Input.GetKeyDown(KeyCode.Mouse1)) _timeCounterForAttack.StartCounter();
            else
            {
                var holdingTime = _timeCounterForAttack.StopCounter();
                if (holdingTime > 0.3f) ActivateButton(InputButtonType.HeavyAttack);
                else ActivateButton(InputButtonType.Attack);
            }
        }
        else
        {
            DeactivateButton(InputButtonType.Shot);
            DeactivateButton(InputButtonType.Attack);
            DeactivateButton(InputButtonType.HeavyAttack);
        }

        if (Input.GetKey(KeyCode.F) || Input.GetKeyUp(KeyCode.F))
        {
            if (Input.GetKeyDown(KeyCode.F)) _timeCounterForProtect.StartCounter();

            float holdingTime = _timeCounterForProtect.GetTime();

            if (Input.GetKeyUp(KeyCode.F))
            {
                holdingTime = _timeCounterForProtect.StopCounter();

                if (holdingTime <= 0.2f) ActivateButton(InputButtonType.Parrying);
            }

            if (holdingTime > 0.2f) ActivateButton(InputButtonType.Shield);
        }
        else
        {
            DeactivateButton(InputButtonType.Shield);
            DeactivateButton(InputButtonType.Parrying);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            ActivateButton(InputButtonType.Dodge);
        }
        else
        {
            DeactivateButton(InputButtonType.Dodge);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            ActivateButton(InputButtonType.Interact);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            ActivateButton(InputButtonType.Heal);
        }
        else
        {
            DeactivateButton(InputButtonType.Interact);
            DeactivateButton(InputButtonType.Heal);
        }

        if (Input.GetKeyDown(KeyCode.Escape)) ActivateButton(InputButtonType.Pause);
        else DeactivateButton(InputButtonType.Pause);
    }
}