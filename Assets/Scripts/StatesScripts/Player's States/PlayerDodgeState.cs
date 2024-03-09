using Cysharp.Threading.Tasks;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDodgeState", menuName = "Player's State/DodgeState")]
public class PlayerDodgeState : PlayerState
{
    [SerializeField] private float _neededStamina;
    [SerializeField] private float _dodgeTime;
    [SerializeField] private float _dodgeDistance;
    private Rigidbody2D _rb;
    private float _speed;

    private Vector2 _direction;

    private CancellationToken _token;
    private PauseTokenSource _pauseTokenSource;
    private PauseToken _pauseToken;

    public override void Initialize(Player player, PauseService pauseService)
    {
        base.Initialize(player, pauseService);
        _rb = player.GetComponent<Rigidbody2D>();

        _token = player.GetCancellationTokenOnDestroy();
        _pauseTokenSource = new PauseTokenSource();
        _pauseToken = _pauseTokenSource.Token;
    }

    public override void Enter()
    {
        base.Enter();
        _player.UseStamina(_neededStamina);
        IsStateFinished = false;
        _player.IsImmortal = true;
        _direction = new Vector2(_player.transform.localScale.x, 0).normalized;

        ChangeDodgeSpeed();
    }

    public override void Update()
    {
        _rb.velocity = _direction * _speed;
        base.Update();
    }

    private async void ChangeDodgeSpeed()
    {
        var current = 0f;
        _speed = _dodgeDistance / _dodgeTime;
        var acceleration = (0 - _speed) / _dodgeTime;
        while (current < _dodgeTime)
        {
            _speed = _dodgeDistance / _dodgeTime + (current / _dodgeTime) * acceleration;
            _speed *= 2;
            current += Time.unscaledDeltaTime;
            await UniTask.Yield();
            if (_token.IsCancellationRequested) return;
            if (_pauseToken.IsCancellationRequested) await UniTask.WaitUntil(() => !_pauseToken.IsCancellationRequested);
        }
        _speed = 0f;
        IsStateFinished = true;
    }

    public override void Exit()
    {
        _player.IsImmortal = false;
        base.Exit();
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

    public override bool IsStateAvailable()
    {
        return _player.GetStamina() >= _neededStamina;
    }
}
