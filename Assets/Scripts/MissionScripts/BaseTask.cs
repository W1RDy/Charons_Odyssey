using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTask : MonoBehaviour
{
    [SerializeField] protected int _index;
    [SerializeField] protected Goals _goals;
    [SerializeField] protected TaskInfo _taskInfo;

    public int Index { get => _index; }
    public event Action TaskIsFinished;

    public virtual void ActivateTask()
    {
        if (gameObject.activeInHierarchy && _goals && _goals.gameObject.activeInHierarchy)
        {
            Debug.Log(_goals);
            _goals.ActivateGoal(_taskInfo.targetText);
        }
    }

    public virtual void FinishTask()
    {
        TaskIsFinished?.Invoke();
    }
}
