using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;

public class TaskTalk : BaseTask
{
    [SerializeField] private string _goalDialogIndex;
    [SerializeField] private string _npcIndex;
    private DialogManager _dialogManager;
    private DialogUpdater _dialogUpdater;
    private Action<string> CheckDialog;

    [Inject]
    private void Construct(DialogManager dialogManager, DialogUpdater dialogUpdater)
    {
        _dialogManager = dialogManager;
        _dialogUpdater = dialogUpdater;
        CheckDialog = dialogIndex =>
        {
            if (dialogIndex == _goalDialogIndex) FinishTask();
        };

        _dialogManager.DeactivateDialog += CheckDialog;
    }

    public override void ActivateTask()
    {
        base.ActivateTask();
        _dialogUpdater.UpdateDialog(_npcIndex, _goalDialogIndex);
    }

    private void OnDestroy()
    {
        _dialogManager.DeactivateDialog -= CheckDialog;
    }
}
