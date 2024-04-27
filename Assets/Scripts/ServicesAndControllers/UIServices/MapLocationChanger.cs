using System;
using TMPro;
using UnityEngine;
using Zenject;

public class MapLocationChanger : SubscribableClass
{
    private LocationIndicator _locationIndicator;
    private MapShip _mapShip;

    private ExitToStationTrigger _exitToStationTrigger;

    private Action<MapLocation> ChangeLocationDelegate;

    [Inject]
    private void Construct(MapShip mapShip)
    {
        _mapShip = mapShip;

        Subscribe();
    }

    public MapLocationChanger(LocationIndicator locationIndicator, ExitToStationTrigger exitToStationTrigger)
    {
        _locationIndicator = locationIndicator;
        _exitToStationTrigger = exitToStationTrigger;
    }

    public void ChangeLocation(MapLocation mapLocation)
    {
        if (mapLocation is MapStation mapStation)
        {
            _locationIndicator.ActivateIndicator("Вы прибыли на станцию " + mapStation.Name);
            _exitToStationTrigger.ActivateTrigger(mapStation);
        }
        else
        {
            _locationIndicator.ActivateIndicator(mapLocation.Name);
            if (_exitToStationTrigger.IsActivated) _exitToStationTrigger.DeactivateTrigger();
        }
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