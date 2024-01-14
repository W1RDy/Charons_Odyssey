using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class NPCGroup : MonoBehaviour, ITalkable, IAvailable
{
    [SerializeField] private NPC[] _NPCs;
    [SerializeField] private Trigger _trigger;
    [SerializeField] private float _talkDelay;
    [SerializeField] private string _branchIndex;
    [SerializeField] private bool _isAvailable = true;
    private ITalkable _currentTalkable;
    private DialogActivator _dialogActivator;

    [Inject]
    private void Consruct(DialogActivator dialogActivator)
    {
        _dialogActivator = dialogActivator;
    }

    private void Awake()
    {
        _trigger.TriggerWorked += StartDialog;
    }

    public void Talk(string message)
    {

    }

    public void StartDialog()
    {
        _dialogActivator.ActivateDialog(_branchIndex, _NPCs);
        _trigger.TriggerWorked -= StartDialog;
    }

    public void ChangeAvailable(bool isAvailable)
    {
        _isAvailable = isAvailable;
    }

    public ITalkable[] GetTalkables()
    {
        return _NPCs;
    }

    public string GetTalkableIndex()
    {
        return _NPCs[0].GetTalkableIndex();
    }
}
