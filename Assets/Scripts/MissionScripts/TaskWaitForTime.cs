using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskWaitForTime : BaseTask
{
    [SerializeField] float _timeForWaiting;

    public override void ActivateTask()
    {
        base.ActivateTask();
        var timer = new Timer();
        StartCoroutine(timer.TimeCounterCoroutine(_timeForWaiting, () => FinishTask()));
    }
}
