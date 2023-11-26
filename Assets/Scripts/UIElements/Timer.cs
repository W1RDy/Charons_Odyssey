using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Timer : MonoBehaviour
{
    private Text _indicator;
    private int _time;
    private bool _isWorked;

    private void Awake()
    {
        _indicator = GetComponent<Text>();
        StartTimer();
    }

    private void SetTimer()
    {
        _indicator.text = _time.ToString();
    }

    private IEnumerator TimerCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (!_isWorked) break;
            _time += 1;
            SetTimer();
        }
    }

    public void StopTimer()
    {
        _isWorked = false;
    }

    public void StartTimer()
    {
        _isWorked = true;
        StartCoroutine(TimerCoroutine());
    }
}
