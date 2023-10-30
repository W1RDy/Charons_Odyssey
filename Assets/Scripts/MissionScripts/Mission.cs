using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission : MonoBehaviour
{
    public BaseTask[] tasks;
    private BaseTask _currentTask;

    private void Start()
    {
        _currentTask = tasks[0];
        _currentTask.ActivateTask();
    }

    public void ActivateNextTask()
    {
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
        LoadSceneManager.Instance.LoadNextLevel();
    }
}
