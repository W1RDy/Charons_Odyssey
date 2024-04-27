using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Zenject;

public class DialogLifeController : SubscribableClass, IPause
{
    private bool _dialogIsFinished = true;

    private TalkableFinderOnLevel _talkableFinder;
    private DialogBranch _currentDialog;
    private DialogService _dialogService;
    private DialogClickHandler _dialogClickHandler;

    private PauseService _pauseService;

    public event Action<string> DialogDeactivated;

    [Inject]
    private void Construct(TalkableFinderOnLevel talkableFinder, DialogService dialogService, PauseService pauseService)
    {
        _talkableFinder = talkableFinder;
        _dialogService = dialogService;
        _pauseService = pauseService;
    }

    public DialogLifeController(DialogClickHandler dialogClickHandler)
    {
        _dialogClickHandler = dialogClickHandler;
        Subscribe();
    }

    public void StartDialog(string branchIndex)
    {
        if (_currentDialog == null)
        {
            _pauseService.AddPauseObj(this);

            _currentDialog = _dialogService.GetDialog(branchIndex);
            _dialogIsFinished = false;

            var message = _currentDialog.GetFirstMessage();
            var talkable = _talkableFinder.GetTalkable(message.talkableIndex);
            talkable.Talk(message.message);

            _dialogClickHandler.Activate();
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
        _dialogClickHandler.Deactivate();
       
        _dialogIsFinished = true;

        DialogDeactivated(_currentDialog.index);
        _currentDialog = null;
    }

    public bool DialogIsFinished() => _dialogIsFinished;

    public void Pause()
    {
        _dialogClickHandler.Deactivate();
    }

    public void Unpause()
    {
        _dialogClickHandler.Activate();
    }

    public override void Subscribe()
    {
        _dialogClickHandler.OnClick += ActivateNextMessage;
    }

    public override void Unsubscribe()
    {
        _dialogClickHandler.OnClick -= ActivateNextMessage;
    }
}
