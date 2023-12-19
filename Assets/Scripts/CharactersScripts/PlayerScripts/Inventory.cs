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

    public void AddItem(ItemConfig _item)
    {
        _items.Add(_item.type, _item);
        InventoryUpdated?.Invoke();
    }

    public void RemoveItem(ItemType _itemType)
    {
        _items.Remove(_itemType);
        InventoryUpdated?.Invoke();
    }

    public bool HasItem(ItemType _itemType)
    {
        return _items.ContainsKey(_itemType);
    }
}

[Serializable]
public class ItemConfig
{
    public ItemType type;
    public Item itemObject;

    public ItemConfig(Item _itemObject)
    {
        type = _itemObject.type;
        itemObject = _itemObject;
    }
}

public enum ItemType
{
    MissionItem,
    Key
}
