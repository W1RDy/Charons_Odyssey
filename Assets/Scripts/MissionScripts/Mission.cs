using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Mission : MonoBehaviour
{
    [SerializeField] private BaseTask[] _tasks;
    [SerializeField] private Timer _timer;
    [SerializeField] private GameService _gameService;
    private BaseTask _currentTask;
    protected CancellationToken _token;

    private async void Start()
    {
        _currentTask = _tasks[0];
        _token = this.GetCancellationTokenOnDestroy();
        foreach (var task in _tasks) task.TaskIsFinished += ActivateNextTask;

        await Delayer.Delay(1f, _token);
        if (_token.IsCancellationRequested) return;
        _currentTask.ActivateTask();
    }

    public void ActivateNextTask()
    {
        _currentTask.TaskIsFinished -= ActivateNextTask;
        var index = _currentTask.Index + 1;

        if (index >= _tasks.Length) FinishMission();
        else
        {
            _currentTask = _tasks[index];
            _currentTask.ActivateTask();
        }
    }

    public void FinishMission()
    {
        if (_timer != null) _timer.StopTimer();
        _gameService.WinGame();
    }

    public void OnDestroy()
    {
        _currentTask.TaskIsFinished -= ActivateNextTask;
    }
}
