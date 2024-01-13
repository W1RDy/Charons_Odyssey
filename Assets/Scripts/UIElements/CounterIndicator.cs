using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class CounterIndicator : MonoBehaviour
{
    private Text _text;

    protected virtual void Awake()
    {
        _text = GetComponentInChildren<Text>();
    }

    public virtual void SetCount(int count)
    {
        _text.text = "x" + count;
    }
}
