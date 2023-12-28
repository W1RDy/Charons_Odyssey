using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsCounterIndicator : CounterIndicator
{
    [SerializeField] private ItemType _itemType;

    protected override void Awake()
    {
        base.Awake();
        Inventory.Instance.InventoryUpdated += UpdateIndicator;
    }

    private void UpdateIndicator()
    {
        SetCount(Inventory.Instance.GetItemCount(_itemType));
    }

    private void OnDestroy()
    {
        Inventory.Instance.InventoryUpdated -= UpdateIndicator;
    }
}
