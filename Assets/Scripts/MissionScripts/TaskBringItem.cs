using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskBringItem : BaseTask
{
    [SerializeField] private Trigger _bringPlace;

    private void Awake()
    {
        _bringPlace.TriggerWorked += CheckItemInInventory;
    }

    private void CheckItemInInventory()
    {
        if (Inventory.Instance.HasItem(ItemType.MissionItem))
        {
            FinishTask();
            _bringPlace.TriggerWorked -= CheckItemInInventory;
            Inventory.Instance.RemoveItem(ItemType.MissionItem);
        }
        else Debug.Log("Item isn't collected!");
    }

    private void OnDestroy()
    {
        _bringPlace.TriggerWorked -= CheckItemInInventory;
    }
}
