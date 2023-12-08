using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCGroup : MonoBehaviour, ITalkableGroup
{
    [SerializeField] private NPC[] _NPCs;
    [SerializeField] private Trigger _trigger;
    [SerializeField] private float _talkDelay;
    [SerializeField] private string _branchIndex;
    private ITalkable _currentTalkable;

    private void Awake()
    {
        _trigger.TriggerWorked += Talk;
    }

    public void ChangeTalkable(string _talkableIndex)
    {
        _currentTalkable = FindTalkable(_talkableIndex);
    }

    public Vector2 GetTalkableTopPoint()
    {
        return _currentTalkable.GetTalkableTopPoint();
    }

    public void Talk()
    {
        _currentTalkable = _NPCs[0]; // исправить обязательно
        DialogManager.Instance.StartDialog(_branchIndex, this, _talkDelay);
        _trigger.TriggerWorked -= Talk;
    }

    private ITalkable FindTalkable(string _index)
    {
        foreach (var NPC in _NPCs)
        {
            if (NPC.GetTalkableIndex() == _index) return NPC;
        }
        throw new ArgumentNullException("NPC with index " + _index + " isn't found!");
    }
}
