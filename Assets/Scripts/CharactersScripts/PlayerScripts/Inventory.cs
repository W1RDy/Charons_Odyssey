using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Inventory : MonoBehaviour
{
    private Dictionary<ItemType, ItemConfig> _items;
    public event Action InventoryUpdated;

    private void Awake()
    {
        _items = new Dictionary<ItemType, ItemConfig>();
    }

    public void AddItem(Item item)
    {
        ItemConfig itemConfig;
        if (!_items.ContainsKey(item.type))
        {
            itemConfig = new ItemConfig(item);
            _items.Add(item.type, itemConfig);
        }
        else itemConfig = _items[item.type];

        itemConfig.count++;
        InventoryUpdated?.Invoke();
    }

    public void RemoveItem(ItemType itemType)
    {
        if (_items.ContainsKey(itemType))
        {
            var item = _items[itemType];
            item.count--;
            if (item.count < 0) item.count = 0;
            InventoryUpdated?.Invoke();
        }
    }

    public bool HasItem(ItemType itemType)
    {
        return GetItemCount(itemType) > 0;
    }

    public int GetItemCount(ItemType itemType)
    {
        if (_items.ContainsKey(itemType)) return _items[itemType].count;
        return 0;
    }
}

[Serializable]
public class ItemConfig
{
    public ItemType type;
    public Item itemObject;
    public int count = 0;

    public ItemConfig(Item itemObject)
    {
        type = itemObject.type;
        this.itemObject = itemObject;
    }
}

public enum ItemType
{
    MissionItem,
    Key,
    FirstAidKit
}
