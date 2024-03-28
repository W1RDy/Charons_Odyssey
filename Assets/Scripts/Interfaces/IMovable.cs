using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public interface IMovable 
{
    public void Move();

    public void SetSpeed(float speed);
}

public interface IMovableWithFlips : IMovable
{
    public void Flip(Vector2 direction);
}

public interface IMovableWithStops : IMovable
{
    public void StartMove();
    public void StopMove();
}

public interface INavMeshMovable
{
    public void Move(NavMeshPath path);
}