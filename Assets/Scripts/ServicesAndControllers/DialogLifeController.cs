using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;

public class DialogLifeController
{
    private bool _dialogIsFinished = true;
    public event Action<string> DeactivateDialog;
    private DialogBranch _currentDialog;
    private TalkableFinderOnLevel _talkableFinder;
    private DialogService _dialogService;

    [Inject]
    private void Construct(TalkableFinderOnLevel talkableFinder, DialogService dialogService)
    {
        _talkableFinder = talkableFinder;
        _dialogService = dialogService;
    }

    public void StartDialog(string branchIndex, CancellationToken token)
    {
        if (_currentDialog == null)
        {
            _currentDialog = _dialogService.GetDialog(branchIndex);
            _dialogIsFinished = false;
            var message = _currentDialog.GetFirstMessage();
            var talkable = _talkableFinder.GetTalkable(message.talkableIndex);
            talkable.Talk(message.message);
            Dialog(2f, token);
        }
    }

    public async void Dialog(float delay, CancellationToken token)
    {
        while (true)
        {
            await Delayer.Delay(delay, token);
            if (token.IsCancellationRequested || _dialogIsFinished) break;
            ActivateNextMessage();
        }
    }

    private void ActivateNextMessage()
    {
        var message = _currentDialog.GetNextMessage();
        if (message == null)
        {
            FinishDialog();
            return;
        }

        var talkable = _talkableFinder.GetTalkable(message.talkableIndex);
        talkable.Talk(message.message);
    }

    public void FinishDialog()
    {
        _dialogIsFinished = true;
        DeactivateDialog(_currentDialog.index);
        _currentDialog = null;
    }

    public bool DialogIsFinished() => _dialogIsFinished;
}
