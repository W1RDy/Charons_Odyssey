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
