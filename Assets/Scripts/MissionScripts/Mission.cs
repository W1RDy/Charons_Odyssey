using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;

public class Mission : MonoBehaviour
{
    [SerializeField] private BaseTask[] _tasks;
    private BaseTask _currentTask;

    [SerializeField] private Timer _timer;
    private GameStateController _gameLifeController;

    protected CancellationToken _token;

    [SerializeField] private bool _isWinWhenCompletedMissions;

    [Inject]
    private void Construct(GameStateController gameLifeController)
    {
        _gameLifeController = gameLifeController;
    }

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
        if (_isWinWhenCompletedMissions) _gameLifeController.ActivateWinState();
    }

    public void OnDestroy()
    {
        _currentTask.TaskIsFinished -= ActivateNextTask;
    }
}
