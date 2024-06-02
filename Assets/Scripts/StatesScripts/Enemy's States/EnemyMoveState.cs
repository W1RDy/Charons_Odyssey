using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "Move State", menuName = "Enemy's State/Move State")]
public class EnemyMoveState : EnemyState
{
    protected EnemyMove _movable;
    private Vector2 _movePosition;
    private bool _isMoving;

    public override void Initialize(Enemy enemy, IInstantiator instantiator)
    {
        base.Initialize(enemy, instantiator);
        _movable = enemy.GetComponent<EnemyMove>();
    }

    public override void Enter()
    {
        base.Enter();
        IsStateFinished = false;
        _enemy.SetAnimation("Move", true);
        _isMoving = true;
    }

    public void SetMovePosition(Vector2 movePosition)
    {
        _movePosition = movePosition;
        _isMoving = true;
    }

    public override void FixedUpdate()
    {
        if (_isMoving)
        {
            var direction = new Vector2(_movePosition.x - _enemy.transform.position.x, 0).normalized;
            if (Mathf.Abs(_movePosition.x - _enemy.transform.position.x) < 0.1f) IsStateFinished = true;
            _movable.Move(direction);
            base.FixedUpdate();
        }
    }

    public override void Exit()
    {
        base.Exit();
        IsStateFinished = true;
        _enemy.SetAnimation("Move", false);
    }

    public override void ResetValues()
    {
        base.ResetValues();
        _isMoving = false;
    }
}