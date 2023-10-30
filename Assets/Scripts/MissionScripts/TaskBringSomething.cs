using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskBringSomething : BaseTask
{
    [SerializeField] BringPlace bringPlace;
    bool _isItemCollected;

    public override void ActivateTask()
    {
        base.ActivateTask();
        _isItemCollected = true;
    }

    public override void FinishTask()
    {
        if (_isItemCollected) base.FinishTask();
        else Debug.Log("Item isn't collected!");
    }
}
