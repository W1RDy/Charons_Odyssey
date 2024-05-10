using System;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class MapWayMovementController : MonoBehaviour, ISubscribable
{
    private ClickHandler _clickHandler;

    private WayView _wayView;
    private MapShip _mapShip;

    private ShipMovementView _shipMovementView;

    private bool _isMovementActivated;

    private NavMeshPath _path;
    private Vector2? _destination;

    private Action<Vector2> ActivateMovementDelegate;

    private PauseToken _pausedToken;

    [Inject]
    private void Construct(MapShip mapShip, PauseService pauseService)
    {
        _mapShip = mapShip;

        var pauseHandler = new PauseHandler(pauseService, Pause, Unpause);
        _pausedToken = pauseHandler.GetPauseToken();
    }

    public void Init(ClickHandler clickHandler, WayView wayView, ShipMovementView shipMovementView)
    {
        _clickHandler = clickHandler;
        _wayView = wayView;
        _shipMovementView = shipMovementView;

        Subscribe();
    }

    private void Update()
    {
        if (!_pausedToken.IsCancellationRequested)
        {
            if (_isMovementActivated && _mapShip.DestinationReached())
            {
                DeactivateShipMovement();
            }
        }
    }

    public void TryActivateWayMovement(Vector2 destination)
    {
        Debug.Log(destination);
        _path = ConstructWay(destination);
        if (_path != null)
        {
            _destination = destination;
            ActivateShipMovement(_path);
        }
    }

    private void ActivateShipMovement(NavMeshPath path)
    {
        _mapShip.Move(path);
        _shipMovementView.ActivateMovementView();
        _isMovementActivated = true;
    }

    private NavMeshPath ConstructWay(Vector2 destination)
    {
        if (!_mapShip.Agent.enabled) _mapShip.Agent.enabled = true;

        var path = new NavMeshPath();
        _wayView.CalculateAndShowWay(_mapShip.Agent, path, destination);
        return path;
    }

    public Vector2? GetDestination()
    {
        return _destination;
    }

    private void DeactivateShipMovement()
    {
        _isMovementActivated = false;

        _wayView.ClearView();
        _shipMovementView.DeactivateMovementView();
        _mapShip.Stop();

        _destination = null;
        _path = null;
    }

    public void InteruptMovement()
    {
        if (_isMovementActivated)
        {
            _isMovementActivated = false;
            _wayView.ClearView();
            _mapShip.Stop();
        }
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }

    private void Pause()
    {
        if (_isMovementActivated)
        {
            _isMovementActivated = false;
            _mapShip.Stop();
        }
    }

    private void Unpause()
    {
        if (_path != null)
        {
            _isMovementActivated = true;
            _mapShip.Move(_path);
        }
    }

    public void Subscribe()
    {
        ActivateMovementDelegate = destination => TryActivateWayMovement(destination);

        _clickHandler.OnGoodClick += ActivateMovementDelegate;
    }

    public void Unsubscribe()
    {
        _clickHandler.OnGoodClick -= ActivateMovementDelegate;
    }
}
