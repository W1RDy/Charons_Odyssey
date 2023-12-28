using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    private Dictionary<ItemType, ItemConfig> _items;
    public event Action InventoryUpdated;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            _items = new Dictionary<ItemType, ItemConfig>();
        }
        else Destroy(gameObject);

        DontDestroyOnLoad(Instance);
    }

    public void AddItem(Item _item)
    {
        ItemConfig itemConfig;
        if (!_items.ContainsKey(_item.type))
        {
            itemConfig = new ItemConfig(_item);
            _items.Add(_item.type, itemConfig);
        }
        else itemConfig = _items[_item.type];

        itemConfig.count++;
        InventoryUpdated?.Invoke();
    }

    public void RemoveItem(ItemType _itemType)
    {
        if (_items.ContainsKey(_itemType))
        {
            var item = _items[_itemType];
            item.count--;
            if (item.count < 0) item.count = 0;
            InventoryUpdated?.Invoke();
        }
    }

    public bool HasItem(ItemType _itemType)
    {
        return GetItemCount(_itemType) > 0;
    }

    public int GetItemCount(ItemType _itemType)
    {
        if (_items.ContainsKey(_itemType)) return _items[_itemType].count;
        return 0;
    }
}

[Serializable]
public class ItemConfig
{
    public ItemType type;
    public Item itemObject;
    public int count = 0;

    public ItemConfig(Item _itemObject)
    {
        type = _itemObject.type;
        itemObject = _itemObject;
    }
}

public enum ItemType
{
    MissionItem,
    Key,
    FirstAidKit
}
