using UnityEngine;

public interface IEnemyFactory
{
    public Enemy Create(EnemyType enemyType, Direction direction, bool isAvailable, Vector2 position);
}

public interface INPCFactory
{
    public NPC Create(string npcId, string dialogId, Direction direction, bool isAvailable, Vector2 position);
    public NPC Create(string npcId, string dialogId, Direction direction, bool isAvailable, Vector2 position, Transform parent);
}

public interface INPCGroupFactory
{
    public NPCGroup Create(string[] npcsID, string dialogId, bool isAvailable, Vector2 position);
}

public interface IItemFactory
{
    public Item Create(ItemType itemType, Vector2 position);
}