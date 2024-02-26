using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class NPCTrader : NPC, IInteractable
{
    [SerializeField] private Sprite _traderTradeSprite;
    private WindowActivator _windowActivator;
    private Sprite _defaultSprite;

    [Inject]
    private void Construct(WindowActivator windowActivator)
    {
        _windowActivator = windowActivator;
    }

    public override void InitializeNPC(Direction direction, string dialogId, bool isAvailable)
    {
        base.InitializeNPC(direction, dialogId, isAvailable);
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

    public override void Pause()
    {
        CloseTrade();
        base.Pause();
    }
}
