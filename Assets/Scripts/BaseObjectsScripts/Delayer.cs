using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Delayer
{
    public static IEnumerator DelayCoroutine(float _delay, Action _action)
    {
        yield return new WaitForSeconds(_delay);
        _action();
    }
}
