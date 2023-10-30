using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SmoothChanger<SmoothableType, Type> where SmoothableType : ISmoothable<Type>
{
    public static IEnumerator SmoothChange(SmoothableType startValue, Type secondValue, float duration, Action<Type> callback)
    {
        float current = 0;
        Type firstValue = startValue.GetValue();

        while (current < duration)
        {
            var value = startValue.ChangeValue(firstValue, secondValue, current / duration);
            callback(value);
            current += Time.unscaledDeltaTime;
            yield return null;
        }
        callback(secondValue);
    }
}
