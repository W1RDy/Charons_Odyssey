using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer 
{
    private float _time;
    private bool _isActivated;

    public void StartTimer()
    {
        _time = Time.realtimeSinceStartup;
        _isActivated = true;
    }

    public float StopTimer()
    {
        var totalTime = GetTime();
        _isActivated = false;
        _time = 0;
        return totalTime;
    }

    public float GetTime()
    {
        if (_isActivated) return Time.realtimeSinceStartup - _time;
        return 0;
    }

    public IEnumerator TimeCounterCoroutine (float _duration, Action _action)
    {
        yield return new WaitForSeconds(_duration);
        _action();
    }
}
