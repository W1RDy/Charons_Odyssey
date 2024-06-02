using DG.Tweening;
using System;
using UnityEngine;

[Serializable]
public class TakeHitAnimation : CustomAnimation
{
    [SerializeField] private float _stepDuration = 0.1f;

    private Color _startColor;
    private SpriteRenderer _spriteRenderer;

    public void SetParameters(SpriteRenderer spriteRenderer)
    {
        _spriteRenderer = spriteRenderer;
    }

    public override void Play()
    {
        base.Play();

        _startColor = _spriteRenderer.color;

        _sequence = DOTween.Sequence();

        for (int i = 0; i < 3; i++) 
        {
            _sequence
                .Append(_spriteRenderer.DOColor(Color.red, _stepDuration))
                .Append(_spriteRenderer.DOColor(_startColor, _stepDuration));
        }
        _sequence.AppendCallback(Finish);
    }

    public override void Release()
    {
        base.Release();
        _spriteRenderer.color = _startColor;
    }
}