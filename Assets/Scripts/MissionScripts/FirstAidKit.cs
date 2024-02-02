public class FirstAidKit : InventoryItem
{
    protected override void Awake()
    {
        base.Awake();
        _type = ItemType.FirstAidKit;
    }
}
