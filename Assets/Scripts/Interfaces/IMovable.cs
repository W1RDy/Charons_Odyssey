using Unity.VisualScripting;
using UnityEngine;

public interface IMovable 
{
    public void Move();

    public void SetSpeed(float speed);
}

public interface IMovableWithFlips : IMovable
{
    public void Flip();
}

public interface IMovableWithStops : IMovable
{
    public void StartMove();
    public void StopMove();
}
