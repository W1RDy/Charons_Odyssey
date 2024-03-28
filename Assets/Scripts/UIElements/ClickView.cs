using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ClickView : MonoBehaviour
{
    [SerializeField] protected Transform _view;
    protected SpriteRenderer[] _spriteRenderers;

    protected Vector2 _startScale;
    protected Color _startColor;

    protected Sequence _scaleSequence;
    protected Sequence _colorSequence;

    protected virtual void Awake()
    {
        _spriteRenderers = _view.GetComponentsInChildren<SpriteRenderer>();

        _startScale = _view.localScale;
        _startColor = _spriteRenderers[0].color;
    }

    public virtual void ShowClick()
    {
        gameObject.SetActive(true);

        _scaleSequence = DOTween.Sequence();
        _colorSequence = DOTween.Sequence();
    }

    public virtual void InteraptClick()
    {
        if (_scaleSequence.IsActive())
        {
            _scaleSequence.Kill();
            _colorSequence.Kill();

            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        _view.localScale = _startScale;
        foreach (var spriteRenderer in _spriteRenderers)
        {
            spriteRenderer.color = _startColor;
        }
    }
}
