using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Inventory : MonoBehaviour
{
    private Dictionary<ItemType, ItemConfig> _items;
    private DataController _dataController;
    private Guns gun;
    public event Action InventoryUpdated;

    [Inject]
    private void Construct(DataController dataController, WeaponService weaponService)
    {
        _dataController = dataController;
        gun = weaponService.GetWeapon(WeaponType.Pistol) as Guns;
        InitializeItemsDictionary();
    }

    private void Start()
    {
        LoadInventory();
    }

    private void InitializeItemsDictionary()
    {
        _items = new Dictionary<ItemType, ItemConfig>();

        foreach (var itemType in (Enum.GetValues(typeof(ItemType))) as ItemType[])
        {
            _items.Add(itemType, new ItemConfig(itemType));
        }
    }

    public void AddItem(Item item)
    {
        AddItem(item, 1);
    }

    public void AddItem(Item item, int count)
    {
        var itemConfig = _items[item.Type];
        itemConfig.count += count;
        InventoryUpdated?.Invoke();
        if (item.Type == ItemType.Patrons) gun.PatronsCount = itemConfig.count;
    }

    public void RemoveItem(ItemType itemType)
    {
        RemoveItem(itemType, 1);
    }

    public void RemoveItem(ItemType itemType, int count)
    {
        var item = _items[itemType];
        item.count -= count;
        if (item.count < 0) item.count = 0;
        InventoryUpdated?.Invoke();
        if (itemType == ItemType.Patrons) gun.PatronsCount = item.count;
    }

    public void SaveInventory()
    {
        _dataController.DataContainer.key = _items[ItemType.Key].count;
        _dataController.DataContainer.firstAidKit = _items[ItemType.FirstAidKit].count;
        _dataController.DataContainer.patrons = _items[ItemType.Patrons].count;
        _dataController.SaveDatas();
    }

    public void LoadInventory()
    {
        _items[ItemType.Key].count = _dataController.DataContainer.key;
        _items[ItemType.FirstAidKit].count = _dataController.DataContainer.firstAidKit;
        gun.PatronsCount = _dataController.DataContainer.patrons;
        _items[ItemType.Patrons].count = gun.PatronsCount;
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
    public int count = 0;

    public ItemConfig(ItemType itemType)
    {
        type = itemType;
    }
}

public enum ItemType
{
    MissionItem,
    Key,
    FirstAidKit,
    Patrons
}
