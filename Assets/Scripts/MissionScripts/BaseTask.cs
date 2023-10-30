using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTask : MonoBehaviour 
{
    public int index;
    public TaskInfo taskInfo;
    public Mission mission;

    public virtual void ActivateTask()
    {
        Debug.Log(taskInfo.targetText);
    }

    public virtual void FinishTask()
    {
        mission.ActivateNextTask();
    }
}
