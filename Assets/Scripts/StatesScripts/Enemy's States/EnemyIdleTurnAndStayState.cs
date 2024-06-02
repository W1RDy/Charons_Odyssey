using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "Turn And Stay State", menuName = "Enemy's State/Turn And Stay State")]
public class EnemyIdleTurnAndStayState : EnemyIdleState
{
    [SerializeField] private float _timeBetweenTurns;

    private PauseToken _pauseToken;
    private CancellationToken _cancellationToken;

    private bool _isIdle;

    public override void Initialize(Enemy enemy, IInstantiator instantiator)
    {
        base.Initialize(enemy, instantiator);

        _pauseToken = _pauseHandler.GetPauseToken();
        _cancellationToken = _enemy.GetCancellationTokenOnDestroy();
    }

    public override void Enter()
    {
        base.Enter();
        _isIdle = true;
        WaitUntilTurn();
    }

    private async void WaitUntilTurn()
    {
        while (true)
        {
            if (!_isIdle) break;
            await Delayer.DelayWithPause(_timeBetweenTurns, _cancellationToken, _pauseToken);
            if (!_isIdle || _cancellationToken.IsCancellationRequested) break;
            var flipDirection = _enemy.transform.localScale.x > 0 ? Vector2.left : Vector2.right;
            _enemy.Flip(flipDirection);
        }
    }

    public override void Exit()
    {
        base.Exit();
        _isIdle = false;
    }
}
