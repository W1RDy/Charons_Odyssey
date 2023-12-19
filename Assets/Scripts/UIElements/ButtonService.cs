using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonService : MonoBehaviour
{
    [SerializeField] private WindowActivator _windowActivator;

    public void CloseTradeWindow()
    {
        _windowActivator.DeactivateWindow(WindowType.TradeWindow);
    }
}
