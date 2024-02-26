using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class NPCGroup : MonoBehaviour, ITalkable, IAvailable, IPause
{
    [SerializeField] private Transform[] _NPCPoints;
    [SerializeField] private Trigger _trigger;
    [SerializeField] private float _talkDelay;
    [SerializeField] private string _branchIndex;
    [SerializeField] private bool _isAvailable = true;
    private DialogActivator _dialogActivator;
    private NPC[] _NPCs;
    private PauseService _pauseService;

    [Inject]
    private void Consruct(DialogActivator dialogActivator, PauseService pauseService)
    {
        _dialogActivator = dialogActivator;
        _pauseService = pauseService;
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
        if (_isAvailable)
        {
            _dialogActivator.ActivateDialog(_branchIndex);
            _trigger.TriggerWorked -= StartDialog;
        }
    }

    public void ChangeAvailable(bool isAvailable)
    {
        _isAvailable = isAvailable;
    }

    public string GetTalkableIndex()
    {
        return _NPCs[0].GetTalkableIndex() + ", " + _NPCs[1].GetTalkableIndex();
    }

    public void Pause()
    {
        if (_isAvailable == true) _isAvailable = false;
    }

    public void Unpause()
    {
        if (_isAvailable == false) _isAvailable = true;
    }

    public void OnDestroy()
    {
        _pauseService.RemovePauseObj(this);
    }
}
