using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTrader : NPC, IInteractable
{
    [SerializeField] private WindowActivator _windowActivator;
    [SerializeField] private Sprite _traderTradeSprite;
    private Sprite _defaultSprite;

    protected override void Awake()
    {
        base.Awake();
        _defaultSprite = GetComponent<SpriteRenderer>().sprite;
    }

    public void Interact()
    {
        if (_isAvailable)
        {
            _spriteRenderer.sprite = _traderTradeSprite;
            _windowActivator.ActivateWindowWithDelay(WindowType.TradeWindow, 1f);
        }
    }

    public async void CloseTrade()
    {
        _windowActivator.DeactivateWindow(WindowType.TradeWindow);
        var token = this.GetCancellationTokenOnDestroy();
        await Delayer.Delay(1f, token);
        if (!token.IsCancellationRequested) _spriteRenderer.sprite = _defaultSprite;
    }
}
