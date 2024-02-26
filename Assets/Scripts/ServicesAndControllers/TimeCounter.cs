using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class TimeCounter
{
    private float _time;
    private bool _isActivated;

    public void StartCounter()
    {
        _time = Time.realtimeSinceStartup;
        _isActivated = true;
    }

    public float StopCounter()
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
}
