using System.Collections;
using System.Collections.Generic;
using Zenject;

public class TaskGetItem : BaseTask
{
    private Inventory _inventory;

    [Inject]
    private void Construct(Inventory inventory)
    {
        _inventory = inventory;
    }

    private void Awake()
    {
        _inventory.InventoryUpdated += CheckItemInInventory;
    }

    private void CheckItemInInventory()
    {
        if (_inventory.HasItem(ItemType.MissionItem))
        {
            FinishTask();
            _inventory.InventoryUpdated -= CheckItemInInventory;
        }   
    }

    private void OnDestroy()
    {
        _inventory.InventoryUpdated -= CheckItemInInventory;
    }
}