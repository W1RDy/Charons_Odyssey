using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TaskBringItem : BaseTask
{
    [SerializeField] private Trigger _bringPlace;
    private Inventory _inventory;

    [Inject]
    private void Construct(Inventory inventory)
    {
        _inventory = inventory;
    }

    private void Awake()
    {
        _bringPlace.TriggerWorked += CheckItemInInventory;
    }

    private void CheckItemInInventory()
    {
        if (_inventory.HasItem(ItemType.MissionItem))
        {
            FinishTask();
            _bringPlace.TriggerWorked -= CheckItemInInventory;
            _inventory.RemoveItem(ItemType.MissionItem);
        }
        else Debug.Log("Item isn't collected!");
    }

    private void OnDestroy()
    {
        _bringPlace.TriggerWorked -= CheckItemInInventory;
    }
}
