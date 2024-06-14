using UnityEngine;
using Zenject;

[RequireComponent(typeof(BoxCollider2D))]
public class ExitToStationTrigger : MonoBehaviour, IInteractable
{
    private MapStation _mapStation;
    protected LoadSceneManager _loadSceneManager;
    protected TipActivator _tipActivator;

    public bool IsActivated { get; protected set; }

    [Inject]
    private void Construct(TipActivator tipActivator, LoadSceneManager loadSceneManager)
    {
        _tipActivator = tipActivator;
        _loadSceneManager = loadSceneManager;
    }

    public void ActivateTrigger(MapStation mapStation)
    {
        _mapStation = mapStation;
        gameObject.SetActive(true);
        IsActivated = true;
    }

    public virtual void DeactivateTrigger()
    {
        _mapStation = null;
        gameObject.SetActive(false);
        IsActivated = false;
    }

    public virtual void Interact()
    {
        if (_mapStation != null)
        {
            _loadSceneManager.LoadScene(_mapStation.SceneIndex);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (_mapStation != null)
        {
            _tipActivator.ActivateTip(TipType.ExitToStation);
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (_mapStation != null)
        {
            _tipActivator.DeactivateTip();
        }
    }
}