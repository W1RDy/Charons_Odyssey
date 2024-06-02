using UnityEngine;

public class Gorgon : UnmovableEnemy
{
    [SerializeField] private Transform _shootPoint;

    public Transform ShootPoint => _shootPoint;
}