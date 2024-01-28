using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;

public class TaskTalk : BaseTask
{
    [SerializeField] private string _goalDialogIndex;
    [SerializeField] private string _npcIndex;
    private DialogLifeController _dialogController;
    private DialogUpdater _dialogUpdater;
    private Action<string> CheckDialog;

    [Inject]
    private void Construct(DialogLifeController dialogController, DialogUpdater dialogUpdater)
    {
        _dialogController = dialogController;
        _dialogUpdater = dialogUpdater;
        CheckDialog = dialogIndex =>
        {
            if (dialogIndex == _goalDialogIndex) FinishTask();
        };

        _dialogController.DeactivateDialog += CheckDialog;
    }

    public override void ActivateTask()
    {
        base.ActivateTask();
        _dialogUpdater.UpdateDialog(_npcIndex, _goalDialogIndex);
    }

    private void OnDestroy()
    {
        _dialogController.DeactivateDialog -= CheckDialog;
    }
}
