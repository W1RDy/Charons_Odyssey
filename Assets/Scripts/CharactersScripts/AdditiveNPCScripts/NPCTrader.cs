using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTrader : NPC, IInteractable
{
    [SerializeField] private WindowActivator _windowActivator;

    public void Interact()
    {
        if (_isAvailable)
        {
            _windowActivator.ActivateWindow(WindowType.TradeWindow);
        }
    }
}
