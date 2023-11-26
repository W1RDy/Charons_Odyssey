using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission : MonoBehaviour
{
    public BaseTask[] tasks;
    [SerializeField] private Timer _timer;
    [SerializeField] private GameService _gameService;
    private BaseTask _currentTask;

    private void Start()
    {
        _currentTask = tasks[0];
        _currentTask.ActivateTask();
        foreach (var task in tasks) task.TaskIsFinished += ActivateNextTask;
    }

    public void ActivateNextTask()
    {
        _currentTask.TaskIsFinished -= ActivateNextTask;
        var _index = _currentTask.index + 1;

        if (_index >= tasks.Length) FinishMission();
        else
        {
            _currentTask = tasks[_index];
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
