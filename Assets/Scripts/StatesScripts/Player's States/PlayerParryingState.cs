using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerParryingState", menuName = "Player's State/ParryingState")]
public class PlayerParryingState : PlayerState
{
    [SerializeField] private float _parryingDistance;
    [SerializeField] private int _neededStamina;
    [SerializeField] private float _cooldown;
    private IParryingHittable _parryingHittable;

    private CancellationToken _token;
    private PauseTokenSource _pauseTokenSource;
    private StaminaController _staminaRefiller;

    private bool _isCooldown;

    public void Initialize(Player player, PauseService pauseService, StaminaController staminaRefiller)
    {
        base.Initialize(player, pauseService);
        _token = player.GetCancellationTokenOnDestroy();
        _pauseTokenSource = new PauseTokenSource();
        _staminaRefiller = staminaRefiller;
    }

    public override void Enter()
    {
        IsStateFinished = false;
        _isCooldown = true;
        _staminaRefiller.StopRefillStamina();
        _player.UseStamina(_neededStamina);

        Debug.Log("Parrying");
        _parryingHittable.IsReadyForParrying = false;
        _parryingHittable.ApplyParrying();
        IsStateFinished = true;

        WaitCooldown();
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        _staminaRefiller.StartRefillStamina();
    }

    private async void WaitCooldown()
    {
        await Delayer.DelayWithPause(_cooldown, _token, _pauseTokenSource.Token);
        if (!_token.IsCancellationRequested) _isCooldown = false;
    }

    public override bool IsStateAvailable()
    {
        _parryingHittable = FinderObjects.FindParryingHittableByCircle(_parryingDistance, _player.transform.position);
        return _parryingHittable != null && !_isCooldown && _player.GetStamina() >= _neededStamina;
    }
}