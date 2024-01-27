using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMarker : MonoBehaviour
{
    [SerializeField] private EnemyType _enemyType;
    [SerializeField] private Direction _direction;
    [SerializeField] private bool _isAvailable;
    public EnemyType EnemyType => _enemyType;
    public Direction Direction => _direction;
    public bool IsAvailable => _isAvailable;
}
