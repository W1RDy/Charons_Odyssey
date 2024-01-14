using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Goals : MonoBehaviour
{
    private Text _goals;
    private Action<float> _callback;
    private CancellationToken _token;

    private void Awake()
    {
        _goals = GetComponent<Text>();
        _callback = alphaValue => _goals.color = new Color(_goals.color.r, _goals.color.g, _goals.color.b, alphaValue);
        _token = this.GetCancellationTokenOnDestroy();
    }

    public async void ActivateGoal(string message)
    {
        _callback(0);
        _goals.text = message;
        SetGoals(message);
        await SmoothChanger.SmoothChange(0, 1, 2f, _callback, _token);
        if (_token.IsCancellationRequested) return;
        await Delayer.Delay(2f, _token);
        if (_token.IsCancellationRequested) return;
        await SmoothChanger.SmoothChange(1, 0, 2f, _callback, _token);
    }

    private void SetGoals(string goals)
    {
        _goals.text = goals;
    }
}
