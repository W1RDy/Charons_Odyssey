using DG.Tweening;
using System;
using UnityEngine;

[Serializable]
public class StunAnimation : CustomAnimation
{
    [SerializeField] private float _scaleOffset = 0.5f;
    [SerializeField] private float _stepDuration = 0.02f;

    [SerializeField] private Color _endColor;

    private Vector3 _startScale;
    private Color _startColor;

    private Transform _transform;
    private SpriteRenderer _spriteRenderer;

    private Sequence _colorSequence;

    public void SetParameters(Transform transform, SpriteRenderer spriteRenderer)
    {
        _transform = transform;
        _spriteRenderer = spriteRenderer;
    }

    public override void Play()
    {
        base.Play();
        _startScale = _transform.localScale;
        _startColor = _spriteRenderer.color;

        _sequence = DOTween.Sequence();
        _colorSequence = DOTween.Sequence();

        _sequence
            .Append(_transform.DOScaleX(_startScale.x + _scaleOffset, _stepDuration))
            .Append(_transform.DOScaleX(_startScale.x, _stepDuration));

        _colorSequence
            .Append(_spriteRenderer.DOColor(_endColor, _stepDuration))
            .Append(_spriteRenderer.DOColor(_startColor, _stepDuration));
    }

    public override void Kill()
    {
        if (IsPlaying)
        {
            _colorSequence.Kill();
        }
        base.Kill();
    }

    public override void Release()
    {
        base.Release();
        _transform.localScale = _startScale;
        _spriteRenderer.color = _startColor;
    }
}
