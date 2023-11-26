using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISmoothable<T>
{
    public T ChangeValue(T firstValue, T secondValue, float step);

    public T GetValue();
}
