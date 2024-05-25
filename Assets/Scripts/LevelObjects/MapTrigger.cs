using UnityEngine;
using Zenject;

[RequireComponent(typeof(BoxCollider2D))]
public class MapTrigger : MonoBehaviour, IInteractable, IAvailable
{
    private MapActivator _mapActivator;
    private TipActivator _tipActivator;

    private bool _isAvailable;

    [Inject]
    private void Construct(MapActivator mapActivator, TipActivator tipActivator)
    {
        _mapActivator = mapActivator;
        _tipActivator = tipActivator;
    }

    public void Interact()
    {
        if (_isAvailable)
        {
            if (!_mapActivator.IsActivated)
            {
                _mapActivator.ActivateMap();
            }
            else
            {
                _mapActivator.DeactivateMap();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isAvailable && collision.CompareTag("Player")) _tipActivator.ActivateTip(TipType.OpenMap);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_isAvailable && collision.CompareTag("Player")) _tipActivator.DeactivateTip();
    }

    public void ChangeAvailable(bool isAvailable)
    {
        _isAvailable = isAvailable;
    }
}

