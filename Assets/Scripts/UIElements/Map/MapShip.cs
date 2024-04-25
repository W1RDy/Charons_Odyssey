using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MapShip : MonoBehaviour
{
    [SerializeField] private float _speed;
    private MapShipMoveHandler _moveHandler;
    public NavMeshAgent Agent { get; private set; }

    public event Action<MapLocation> ReachedNewLocation;

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Agent.enabled = false;
        _moveHandler = new MapShipMoveHandler(Agent, _speed);
    }

    public void Move(NavMeshPath path)
    {
        _moveHandler.Move(path);
    }

    public bool DestinationReached()
    {
        return Agent.remainingDistance < 0.1f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("MapLocation") || collision.CompareTag("MapGoalLocation"))
        {
            Debug.Log("NewLocation");
            var location = collision.gameObject.GetComponent<MapLocation>();
            ReachedNewLocation?.Invoke(location);
        }
    }
}
