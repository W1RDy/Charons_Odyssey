using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TaskInfo 
{
    public TaskType type;
    public string targetText;
}

public enum TaskType
{
    HoldOutForTime,
    GetItem,
    BringItem,
    Talk
}
