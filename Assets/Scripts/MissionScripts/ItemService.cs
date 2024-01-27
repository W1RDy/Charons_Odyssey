using UnityEngine;

public class ItemService
{
    #region Const

    private const string LevelItem = "LevelItem";
    private const string FirstAidKit = "FirstAidKit";
    private const string Patrons = "Patrons";
    private const string Key = "Key";

    #endregion

    #region Prefabs

    private Item _levelItemPrefab;
    private Item _firstAidKitPrefab;
    private Item _patronsPrefab;
    private Item _keyPrefab;

    #endregion

    public void InitializeService()
    {
        LoadResources();
    }

    private void LoadResources()
    {
        _levelItemPrefab = Resources.Load<Item>("Item/" + LevelItem);
        _firstAidKitPrefab = Resources.Load<Item>("Item/" + FirstAidKit);
        _patronsPrefab = Resources.Load<Item>("Item/" + Patrons);
        _keyPrefab = Resources.Load<Item>("Item/" + Key);
    }

    public Item GetItemPrefab(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.MissionItem:
                return _levelItemPrefab;
            case ItemType.Key:
                return _keyPrefab;
            case ItemType.FirstAidKit:
                return _firstAidKitPrefab;
            case ItemType.Patrons:
                return _patronsPrefab;
        }
        throw new System.InvalidOperationException("Item with type " + itemType + " doesn't exist!");
    }
}
