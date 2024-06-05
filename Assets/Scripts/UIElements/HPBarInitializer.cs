using UnityEngine;

public class HPBarInitializer
{
    public HPBarFactory _hpBarFactory;
    private float _posYOffset = 0.4f;

    public HPBarInitializer(HPBarFactory hpBarFactory)
    {
        _hpBarFactory = hpBarFactory;
    }

    public EntityHPBar GetHpBar(SpriteRenderer spriteRenderer, float maxHP, Transform entity)
    {
        var spawnPoint = new Vector2(spriteRenderer.bounds.center.x, spriteRenderer.bounds.max.y + _posYOffset);
        var hpBar = _hpBarFactory.Create(spawnPoint) as EntityHPBar;
        hpBar.Init(maxHP, entity);

        return hpBar;
    }
}