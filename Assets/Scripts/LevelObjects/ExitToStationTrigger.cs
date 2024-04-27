using UnityEngine;
using Zenject;

[RequireComponent(typeof(BoxCollider2D))]
public class ExitToStationTrigger : MonoBehaviour, IInteractable
{
    private MapStation _mapStation;
    private LoadSceneManager _loadSceneManager;
    private TipActivator _tipActivator;

    public bool IsActivated { get; private set; }

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

    public void DeactivateTrigger()
    {
        _mapStation = null;
        gameObject.SetActive(false);
        IsActivated = false;
    }

    public void Interact()
    {
        _loadSceneManager.LoadScene(_mapStation.SceneIndex);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _tipActivator.ActivateTip(TipType.ExitToStation);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _tipActivator.DeactivateTip();
    }
}