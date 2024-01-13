using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public static class SmoothChanger
{
    public async static UniTask SmoothChange(float startValue, float endValue, float duration, Action<float> callback, CancellationToken token)
    {
        float current = 0;
        float firstValue = startValue;

        while (current < duration)
        {
            if (token.IsCancellationRequested) return;
            var value = Mathf.Lerp(firstValue, endValue, current/duration);
            callback(value);
            current += Time.unscaledDeltaTime;
            await UniTask.Yield();
        }
        callback(endValue);
    }
}
