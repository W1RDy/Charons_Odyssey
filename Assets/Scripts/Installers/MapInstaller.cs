using System;
using UnityEngine;
using Zenject;

public class MapInstaller : MonoInstaller
{
    [SerializeField] private MapShip _ship;

    [SerializeField] private MapWayMovementController _wayMovementController;

    #region View

    [SerializeField] private LocationIndicator _locationIndicator;
    [SerializeField] private WayView _wayView;
    [SerializeField] private ClickHandler _clickHandler;

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
        _wayMovementController.Init(_clickHandler, _wayView);
        Container.Bind<MapWayMovementController>().FromInstance(_wayMovementController).AsSingle();
    }

    private void BindMapLocationChanger()
    {
        var locationChanger = new MapLocationChanger(_locationIndicator);
        Container.Inject(locationChanger);
        Container.Bind<MapLocationChanger>().FromInstance(locationChanger).AsSingle();
    }
}
