using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MapShipMoveHandler : INavMeshMovable
{
    private NavMeshAgent _agent;

    public MapShipMoveHandler(NavMeshAgent agent, float speed)
    {
        _agent = agent;

        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _agent.speed = speed;
    }

    public void Move(NavMeshPath path)
    {
        Debug.Log(path);
        _agent.SetPath(path);
    }
}
