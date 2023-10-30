using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothableFloat : ISmoothable<float>
{
    float value;

    public SmoothableFloat(float value)
    {
        this.value = value;
    }

    public float ChangeValue(float firstValue, float secondValue, float step)
    {
        return value = Mathf.Lerp(firstValue, secondValue, step);
    }

    public float GetValue()
    {
        return value;
    }
}
