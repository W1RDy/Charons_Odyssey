using UnityEngine;

public class ItemFactory : IItemFactory
{
    private ItemService _itemService;
    private ICustomInstantiator _instantiator;

    public ItemFactory(ICustomInstantiator instantiator, ItemService itemService)
    {
        _instantiator = instantiator;
        _itemService = itemService;
    }

    public Item Create(ItemType itemType, Vector2 position)
    {
        var itemPrefab = _itemService.GetItemPrefab(itemType);
        var item = _instantiator.InstantiatePrefabForComponent<Item>(itemPrefab, position);
        return item;
    }
}
