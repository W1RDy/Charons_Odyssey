using DG.Tweening;
using System;
using UnityEngine;

[Serializable]
public class ScaleWithOffsetAnimation : CustomAnimation
{
    [SerializeField] private float _startScale;
    [SerializeField] private float _endScale;

    [SerializeField] private Vector3 _startOffset;
    [SerializeField] private Vector2 _offset;

    [SerializeField] private float _duration;

    private Transform _transform;
    private Vector3 _startPos;

    private Sequence _offsetSequence;

    public void SetParameters(Transform transform)
    {
        _transform = transform;
    }

    public override void Play()
    {
        Play(null);
    }

    public override void Play(Action onComplete)
    {
        base.Play(onComplete);

        _startPos = _transform.position + _startOffset;
        _transform.position = _startPos;
        _transform.localScale = new Vector3(_startScale, _startScale, 1);

        _sequence = DOTween.Sequence();
        _offsetSequence = DOTween.Sequence();

        _sequence
            .Append(_transform.DOScale(_endScale, _duration))
            .AppendCallback(Finish);

        _offsetSequence
            .Append(_transform.DOMove(_startPos.SumWithoutZCoordinate(_offset), _duration));
    }

    public override void Kill()
    {
        _offsetSequence.Kill();
        base.Kill();
    }

    public override void Release()
    {
        base.Release();
        if (_transform.localScale.x != _endScale)
        {
            _transform.position = _startPos.SumWithoutZCoordinate(_offset);
            _transform.localScale = new Vector3(_endScale, _endScale, 1);
        }
    }
}
