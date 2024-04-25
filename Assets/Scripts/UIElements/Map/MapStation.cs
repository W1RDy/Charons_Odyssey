using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapStation : MapLocation, IGoalMapObject
{
    [SerializeField] private int _sceneIndex;
    [SerializeField] private Transform _stationPoint;

    public int SceneIndex => _sceneIndex;

    public Vector2 GetGoalPosition()
    {
        return _stationPoint.position;
    }
}
