using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MapShip : MonoBehaviour
{
    [SerializeField] private float _speed;

    [SerializeField] private WayView _wayView;

    [SerializeField] ClickHandler _clickHandler;
    private MapShipMoveHandler _moveHandler;

    private NavMeshAgent _agent;

    private Action<Vector2> MoveToDestination;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.enabled = false;
        _moveHandler = new MapShipMoveHandler(_agent, _speed);

        MoveToDestination = destination => SetDestination(destination);
        _clickHandler.OnGoodClick += MoveToDestination;
    }

    public void SetDestination(Vector2 destination)
    {
        if (!_agent.enabled) _agent.enabled = true;

        var path = new NavMeshPath();
        _wayView.CalculateAndShowWay(_agent, path, destination);
        _moveHandler.Move(path);
    }

    private void OnDestroy()
    {
        _clickHandler.OnGoodClick -= MoveToDestination;
    }
}
