using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MapLocationChanger : MonoBehaviour
{
    [SerializeField] private LocationIndicator _locationIndicator;

    [SerializeField] private MapShip _mapShip;
    private LoadSceneManager _loadSceneManager;

    private Action<MapLocation> ChangeLocationDelegate;

    [Inject]
    private void Construct(LoadSceneManager loadSceneManager)
    {
        _loadSceneManager = loadSceneManager;
    }

    private void Awake()
    {
        ChangeLocationDelegate = location => ChangeLocation(location);
        _mapShip.ReachedNewLocation += ChangeLocationDelegate;
    }

    public void ChangeLocation(MapLocation mapLocation)
    {
        if (mapLocation is MapStation mapStation) _loadSceneManager.LoadScene(mapStation.SceneIndex);
        else _locationIndicator.ActivateIndicator(mapLocation.Name);
    }

    private void OnDestroy()
    {
        _mapShip.ReachedNewLocation -= ChangeLocationDelegate;
    }
}
