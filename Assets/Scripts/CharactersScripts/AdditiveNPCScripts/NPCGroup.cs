using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class NPCGroup : MonoBehaviour, ITalkable, IAvailable
{
    [SerializeField] private Transform[] _NPCPoints;
    [SerializeField] private Trigger _trigger;
    [SerializeField] private float _talkDelay;
    [SerializeField] private string _branchIndex;
    [SerializeField] private bool _isAvailable = true;
    private DialogActivator _dialogActivator;
    private NPC[] _NPCs;

    [Inject]
    private void Consruct(DialogActivator dialogActivator)
    {
        _dialogActivator = dialogActivator;
    }

    public void InitializeGroup(NPC[] NPCs, string dialogId, bool isAvailable)
    {
        _NPCs = NPCs;
        _trigger.TriggerWorked += StartDialog;
        _branchIndex = dialogId;
        ChangeAvailable(isAvailable);
    }

    public Transform[] GetNPCPoints()
    {
        return _NPCPoints;
    }

    public void Talk(string message)
    {

    }

    public void StartDialog()
    {
        _dialogActivator.ActivateDialog(_branchIndex);
        _trigger.TriggerWorked -= StartDialog;
    }

    public void ChangeAvailable(bool isAvailable)
    {
        _isAvailable = isAvailable;
    }

    public string GetTalkableIndex()
    {
        return _NPCs[0].GetTalkableIndex() + ", " + _NPCs[1].GetTalkableIndex();
    }
}
