using System;
using UnityEngine;
using Zenject;

public class MapInstaller : MonoInstaller
{
    [SerializeField] private MapShip _ship;

    [SerializeField] private MapWayMovementController _wayMovementController;

    [SerializeField] private ExitToStationTrigger _exitToStationTrigger;

    #region View

    [SerializeField] private LocationIndicator _locationIndicator;
    [SerializeField] private WayView _wayView;
    [SerializeField] private ClickHandler _clickHandler;

    [SerializeField] private Wheel _wheel;
    [SerializeField] private Background _background;

    #endregion

    public override void InstallBindings()
    {
        BindMapShip();

        BindMapMovementController();

        BindMapLocationChanger();
        BindMapActivator();
    }

    private void BindMapShip()
    {
        Container.Bind<MapShip>().FromInstance(_ship).AsSingle();
    }

    private void BindMapActivator()
    {
        var mapActivator = new MapActivator();
        Container.Inject(mapActivator);
        Container.Bind<MapActivator>().FromInstance(mapActivator).AsSingle();
    }

    private void BindMapMovementController()
    {
        var shipMovementView = new ShipMovementView(_wheel, _background);

        _wayMovementController.Init(_clickHandler, _wayView, shipMovementView);
        Container.Bind<MapWayMovementController>().FromInstance(_wayMovementController).AsSingle();
    }

    private void BindMapLocationChanger()
    {
        var locationChanger = new MapLocationChanger(_locationIndicator, _exitToStationTrigger);
        Container.Inject(locationChanger);
        Container.Bind<MapLocationChanger>().FromInstance(locationChanger).AsSingle();
    }
}
