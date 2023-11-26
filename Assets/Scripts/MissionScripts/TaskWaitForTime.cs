using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskWaitForTime : BaseTask
{
    [SerializeField] float _timeForWaiting;

    public override void ActivateTask()
    {
        base.ActivateTask();
        StartCoroutine(Delayer.DelayCoroutine(_timeForWaiting, () => FinishTask()));
    }
}
