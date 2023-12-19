using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskWaitForTime : BaseTask
{
    [SerializeField] float _timeForWaiting;

    public override void ActivateTask()
    {
        if (gameObject.activeInHierarchy)
        {
            base.ActivateTask();
            WaitWhileTimerFinished();
        }
    }

    private async void WaitWhileTimerFinished()
    {
        var token = this.GetCancellationTokenOnDestroy();
        await Delayer.Delay(_timeForWaiting, token);
        if (token.IsCancellationRequested) return;

        FinishTask();
        Debug.Log("Done");
    }
}
