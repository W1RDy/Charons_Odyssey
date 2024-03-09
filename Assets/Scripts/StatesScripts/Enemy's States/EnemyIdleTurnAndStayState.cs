using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

[CreateAssetMenu(fileName = "Turn And Stay State", menuName = "Enemy's State/Turn And Stay State")]
public class EnemyIdleTurnAndStayState : EnemyIdleState
{
    [SerializeField] private float _timeBetweenTurns;

    private PauseTokenSource _pauseTokenSource;
    private PauseToken _pauseToken;
    private CancellationToken _cancellationToken;

    private bool _isIdle;

    public override void Initialize(Enemy enemy, PauseService pauseService)
    {
        base.Initialize(enemy, pauseService);

        _pauseTokenSource = new PauseTokenSource();
        _pauseToken = _pauseTokenSource.Token;
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

    public override void Pause()
    {
        _pauseTokenSource.Pause();
        base.Pause();
    }

    public override void Unpause()
    {
        _pauseTokenSource.Unpause();
        base.Unpause();
    }
}
