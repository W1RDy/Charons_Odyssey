using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonService : MonoBehaviour
{
    [SerializeField] private WindowActivator _windowActivator;
    [SerializeField] private NPCTrader _trader;

    public void CloseTrade()
    {
        _trader.CloseTrade();
    }
}
