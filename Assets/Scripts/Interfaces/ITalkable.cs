using UnityEngine;

public interface ITalkable
{
    public void Talk();
    public Vector2 GetTalkableTopPoint();
}

public interface ITalkableGroup : ITalkable
{
    public void ChangeTalkable(string _talkableIndex);
}