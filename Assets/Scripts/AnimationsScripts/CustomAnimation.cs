using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CustomAnimation
{
    protected Sequence _sequence;

    public bool IsPlaying { get; private set; }

    private Action OnComplete;

    public virtual void Play()
    {
        if (IsPlaying) return;

        IsPlaying = true;
    }

    public virtual void Play(Action onComplete)
    {
        if (IsPlaying) return;

        IsPlaying = true;
        OnComplete = onComplete;
    }

    protected virtual void Finish()
    {
        Release();
        OnComplete?.Invoke();
    }

    public virtual void Kill() 
    {
        if (IsPlaying)
        {
            _sequence.Kill();
            Release();
            OnComplete?.Invoke();
        }
    }

    public virtual void Release()
    {
    
    }
}
