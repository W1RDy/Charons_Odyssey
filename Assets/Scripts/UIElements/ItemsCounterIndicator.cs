using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ItemsCounterIndicator : CounterIndicator
{
    [SerializeField] private ItemType _itemType;
    private Inventory _inventory;

    [Inject]
    private void Construct(Inventory inventory)
    {
        _inventory = inventory;
    }

    protected override void Awake()
    {
        base.Awake();
        _inventory.InventoryUpdated += UpdateIndicator;
    }

    private void Start()
    {
        
    }

    private void UpdateIndicator()
    {
        SetCount(_inventory.GetItemCount(_itemType));
    }

    private void OnDestroy()
    {
        _inventory.InventoryUpdated -= UpdateIndicator;
    }
}
