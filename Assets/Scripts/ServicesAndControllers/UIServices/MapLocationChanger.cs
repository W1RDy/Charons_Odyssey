using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MapLocationChanger : SubscribableClass
{
    private LocationIndicator _locationIndicator;

    private MapShip _mapShip;
    private LoadSceneManager _loadSceneManager;

    private Action<MapLocation> ChangeLocationDelegate;

    [Inject]
    private void Construct(MapShip mapShip, LoadSceneManager loadSceneManager)
    {
        Debug.Log(mapShip);
        _mapShip = mapShip;
        _loadSceneManager = loadSceneManager;

        Subscribe();
    }

    public MapLocationChanger(LocationIndicator locationIndicator)
    {
        _locationIndicator = locationIndicator;
    }

    public void ChangeLocation(MapLocation mapLocation)
    {
        if (mapLocation is MapStation mapStation) _loadSceneManager.LoadScene(mapStation.SceneIndex);
        else _locationIndicator.ActivateIndicator(mapLocation.Name);
    }

    public override void Subscribe()
    {
        ChangeLocationDelegate = location => ChangeLocation(location);
        _mapShip.ReachedNewLocation += ChangeLocationDelegate;
    }

    public override void Unsubscribe()
    {
        _mapShip.ReachedNewLocation -= ChangeLocationDelegate;
    }
}
