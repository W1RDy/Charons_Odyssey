using System;
using UnityEngine;
using UnityEngine.AI;

public class MapWayMovementController : MonoBehaviour 
{
    [SerializeField] private ClickHandler _clickHandler;

    [SerializeField] private WayView _wayView;
    [SerializeField] private MapShip _mapShip;
    private bool _isMovementActivated;

    private NavMeshPath _path;

    private Action<Vector2> ActivateMovementDelegate;

    public void Awake()
    {
        ActivateMovementDelegate = destination => TryActivateWayMovement(destination);

        _clickHandler.OnGoodClick += ActivateMovementDelegate;
    }

    private void Update()
    {
        if (_isMovementActivated && _mapShip.DestinationReached())
        {
            DeactivateShipMovement();
        }
    }

    private void TryActivateWayMovement(Vector2 destination)
    {
        _path = ConstructWay(destination);
        if (_path != null) ActivateShipMovement(_path);
    }

    private void ActivateShipMovement(NavMeshPath path)
    {
        _mapShip.Move(path);
        _isMovementActivated = true;
    }

    private NavMeshPath ConstructWay(Vector2 destination)
    {
        if (!_mapShip.Agent.enabled) _mapShip.Agent.enabled = true;

        var path = new NavMeshPath();
        _wayView.CalculateAndShowWay(_mapShip.Agent, path, destination);
        return path;
    }

    private void DeactivateShipMovement()
    {
        _isMovementActivated = false;
        _path = null;
    }

    private void OnDestroy()
    {
        _clickHandler.OnGoodClick -= ActivateMovementDelegate;
    }
}
