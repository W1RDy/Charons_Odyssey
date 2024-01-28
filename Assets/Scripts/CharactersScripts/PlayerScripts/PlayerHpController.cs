using UnityEngine;
using Zenject;

public class PlayerHpController : MonoBehaviour
{
    private float _maxHp;
    private HpIndicator _hpIndicator;
    private GameService _gameService;

    [Inject]
    private void Construct(HpIndicator hpIndicator, GameService gameService)
    {
        _hpIndicator = hpIndicator;
        _gameService = gameService;
    }

    public void SetMaxHp(float hp)
    {
        _maxHp = hp;
    }

    public void TakeHeal(float healValue, ref float hp)
    {
        hp += healValue;
        if (hp > _maxHp) hp = _maxHp;
        _hpIndicator.SetHp(hp);
    }

    public void TakeHit(float damage, ref float hp)
    {
        hp -= damage;
        if (hp < 0) hp = 0;
        _hpIndicator.SetHp(hp);
    }

    public void Death()
    {
        Destroy(gameObject);
        _gameService.LoseGame();
    }
}
