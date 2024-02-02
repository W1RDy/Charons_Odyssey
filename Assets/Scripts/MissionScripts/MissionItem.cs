public class MissionItem : InventoryItem
{
    protected override void Awake()
    {
        base.Awake();
        _type = ItemType.MissionItem;
    }
}
