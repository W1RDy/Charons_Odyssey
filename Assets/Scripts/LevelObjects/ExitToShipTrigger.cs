using System;
using UnityEngine;

public class ExitToShipTrigger : ExitToStationTrigger
{
    public event Action TriggerInteracted;

    public void ActivateTrigger()
    {
        gameObject.SetActive(true);
        IsActivated = true;
    }

    public override void DeactivateTrigger()
    {
        gameObject.SetActive(false);
        IsActivated = false;
    }

    public override void Interact()
    {
        TriggerInteracted?.Invoke();
        _loadSceneManager.LoadShipScene();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        _tipActivator.ActivateTip(TipType.ExitToShip);
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        _tipActivator.DeactivateTip();
    }
}
