using Unity.VisualScripting;
using UnityEngine;

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
