using UnityEngine;
using Zenject;

[RequireComponent(typeof(BoxCollider2D))]
public class MapTrigger : MonoBehaviour, IInteractable
{
    private MapActivator _mapActivator;
    private TipActivator _tipActivator;

    [Inject]
    private void Construct(MapActivator mapActivator, TipActivator tipActivator)
    {
        _mapActivator = mapActivator;
        _tipActivator = tipActivator;
    }

    public void Interact()
    {
        if (!_mapActivator.IsActivated) _mapActivator.ActivateMap();
        else _mapActivator.DeactivateMap();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _tipActivator.ActivateTip(TipType.OpenMap);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _tipActivator.DeactivateTip();
    }
}

