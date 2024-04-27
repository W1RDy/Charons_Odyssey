using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class FadeTextAnimation : CustomAnimation
{
    [SerializeField] private float _endFade;
    [SerializeField] private float _startFade;

    [SerializeField] private float _fadeDuration;

    private Text _text;

    public void SetParameters(Text text)
    {
        _text = text;
    }

    public override void Play()
    {
        base.Play();
        
        _sequence = DOTween.Sequence();
        _sequence
            .Append(_text.DOFade(_endFade, _fadeDuration))
            .AppendCallback(Finish);
    }

    public override void Release()
    {
        base.Release();
        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, _endFade);
    }
}
