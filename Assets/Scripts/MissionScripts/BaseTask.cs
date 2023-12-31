using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTask : MonoBehaviour
{
    public int index;
    public Goals goals;
    public TaskInfo taskInfo;
    public event Action TaskIsFinished;

    public virtual void ActivateTask()
    {
        if (gameObject.activeInHierarchy && goals && goals.gameObject.activeInHierarchy)
        {
            goals.SetGoals(taskInfo.targetText);
        }
    }

    public virtual void FinishTask()
    {
        TaskIsFinished?.Invoke();
    }
}
