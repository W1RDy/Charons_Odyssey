using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;

public class DialogLifeController : IPause
{
    private bool _dialogIsFinished = true;
    public event Action<string> DeactivateDialog;
    private DialogBranch _currentDialog;
    private TalkableFinderOnLevel _talkableFinder;
    private DialogService _dialogService;
    private PauseService _pauseService;
    private PauseTokenSource _pauseTokenSource;
    private PauseToken _pauseToken;

    [Inject]
    private void Construct(TalkableFinderOnLevel talkableFinder, DialogService dialogService, PauseService pauseService)
    {
        _talkableFinder = talkableFinder;
        _dialogService = dialogService;
        _pauseService = pauseService;
        _pauseTokenSource = new PauseTokenSource();
        _pauseToken = _pauseTokenSource.Token;
    }

    public void StartDialog(string branchIndex, CancellationToken token)
    {
        if (_currentDialog == null)
        {
            _pauseService.AddPauseObj(this);
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
            await Delayer.DelayWithPause(delay, token, _pauseToken);
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
        _pauseService.RemovePauseObj(this);
        _dialogIsFinished = true;
        DeactivateDialog(_currentDialog.index);
        _currentDialog = null;
    }

    public bool DialogIsFinished() => _dialogIsFinished;

    public void Pause()
    {
        _pauseTokenSource.Pause();
    }

    public void Unpause()
    {
        _pauseTokenSource.Unpause();
    }
}
