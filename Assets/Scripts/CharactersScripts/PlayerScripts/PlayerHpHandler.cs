using UnityEngine;
using Zenject;

public class PlayerHpHandler : MonoBehaviour
{
    private float _maxHp;
    private HpIndicator _hpIndicator;
    private GameLifeController _gameLifeController;
    private Shield _shield;

    [Inject]
    private void Construct(HpIndicator hpIndicator, GameLifeController gameLifeController)
    {
        _hpIndicator = hpIndicator;
        _gameLifeController = gameLifeController;
    }

    public void Initialize(float hp, Shield shield)
    {
        _maxHp = hp;
        _shield = shield;
    }

    public void TakeHeal(float healValue, ref float hp)
    {
        hp += healValue;
        if (hp > _maxHp) hp = _maxHp;
        _hpIndicator.SetHp(hp);
    }

    public void TakeHit(float damage, ref float hp)
    {
        if (_shield.IsActivated) _shield.AbsorbDamage(ref damage);
        hp -= damage;
        if (hp < 0) hp = 0;
        _hpIndicator.SetHp(hp);
    }

    public void Death()
    {
        Destroy(gameObject);
        _gameLifeController.LoseGame();
    }
}
