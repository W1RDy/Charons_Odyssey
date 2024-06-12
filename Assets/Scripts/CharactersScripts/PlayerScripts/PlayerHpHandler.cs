using UnityEngine;
using Zenject;

public class PlayerHpHandler : MonoBehaviour
{
    private float _maxHp;
    private PlayerHpBar _hpIndicator;
    private GameStateController _gameLifeController;
    private Shield _shield;
    private Player _player;

    private AudioMaster _audioMaster;

    [Inject]
    private void Construct(PlayerHpBar hpIndicator, GameStateController gameLifeController, AudioMaster audioMaster)
    {
        _hpIndicator = hpIndicator;
        _gameLifeController = gameLifeController;

        _audioMaster = audioMaster;
    }

    public void Initialize(float hp, Shield shield, Player player)
    {
        _maxHp = hp;
        _shield = shield;

        _player = player;
    }

    public void TakeHeal(float healValue, ref float hp)
    {
        hp += healValue;
        if (hp > _maxHp) hp = _maxHp;
        _hpIndicator.SetHP(hp);
    }

    public void TakeHit(float damage, ref float hp)
    {
        hp -= damage;
        if (hp < 0) hp = 0;
        _hpIndicator.SetHP(hp);
    }

    public void TakeHit(HitInfo hitInfo, ref float hp)
    {
        var damage = hitInfo.Damage;

        if (IsShieldWorked(hitInfo.DamageDirection))
        {
            if (!hitInfo.IsHasEffect(AdditiveHitEffect.Stun))
            {
                _shield.AbsorbDamage(ref damage);
            }
            _audioMaster.PlaySound("ShieldTakeDamage");
        }
        else
        {
            if (hitInfo.IsHasEffect(AdditiveHitEffect.Stun)) _player.ApplyStun();

            _audioMaster.PlaySound("TakeDamage");
        }
        TakeHit(damage, ref hp);
    }

    private bool IsShieldWorked(Vector2 damageDirection)
    {
        return _shield.IsActivated && ((_shield.IsTurnedRight && damageDirection.x < 0) || (!_shield.IsTurnedRight && damageDirection.x > 0));
    }

    public void Death()
    {
        Destroy(gameObject);
        _gameLifeController.ActivateLoseState();
    }
}
