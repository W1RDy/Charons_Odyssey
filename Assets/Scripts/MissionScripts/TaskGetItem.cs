using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskGetItem : BaseTask
{
    private void Awake()
    {
        Inventory.Instance.InventoryUpdated += CheckItemInInventory;
    }

    private void CheckItemInInventory()
    {
        if (Inventory.Instance.HasItem(ItemType.MissionItem))
        {
            FinishTask();
            Inventory.Instance.InventoryUpdated -= CheckItemInInventory;
        }
    }

    private void OnDestroy()
    {
        Inventory.Instance.InventoryUpdated -= CheckItemInInventory;
    }
}
