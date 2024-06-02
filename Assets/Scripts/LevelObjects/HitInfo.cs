using UnityEngine;

public class HitInfo
{
    private float _damage;
    private AdditiveHitEffect[] _effects;

    private Vector2 _damageDirection;

    public float Damage => _damage;
    public Vector2 DamageDirection => _damageDirection;

    public ReclineInfo ReclineInfo { get; private set; }

    public HitInfo(float damage, Vector2 damageDirection, params AdditiveHitEffect[] effects)
    {
        _damage = damage;
        _damageDirection = damageDirection;
        _effects = effects;
    }

    public bool IsHasEffect(AdditiveHitEffect neededEffect)
    {
        foreach (var effect in _effects)
        {
            if (effect == neededEffect) return true;
        }
        return false;
    }

    public void SetReclineInfo(ReclineInfo reclineInfo)
    {
        ReclineInfo = reclineInfo;
    }
}

public class ReclineInfo
{
    public readonly Transform ReclinePoint;
    public readonly float ReclineForce;

    public ReclineInfo(Transform reclinePoint, float reclineForce)
    {
        ReclinePoint = reclinePoint;
        ReclineForce = reclineForce;
    }
}

public enum AdditiveHitEffect
{
    Recline,
    Stun
}