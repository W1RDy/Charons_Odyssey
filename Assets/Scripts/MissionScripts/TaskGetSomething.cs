using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskGetSomething : BaseTask
{
    [SerializeField] private Item _item;
    private bool _itemIsCollected;

    public override void FinishTask()
    {
        _itemIsCollected = true;
        base.FinishTask();
    }
}
