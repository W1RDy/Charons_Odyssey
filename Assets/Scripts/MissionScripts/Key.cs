public class Key : InventoryItem
{
    protected override void Awake()
    {
        base.Awake();
        _type = ItemType.Key;
    }
}
