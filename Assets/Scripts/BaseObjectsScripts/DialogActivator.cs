using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DialogActivator : IDisposable
{
    private Player _player;
    private DialogManager _dialogManager;
    private DialogCloudService _dialogCloudService;
    private TalkableFinderOnLevel _talkableFinder;
    private Action<string> DeactivateDialogDelegate;

    [Inject]
    private void Construct(Player player, DialogManager dialogManager, DialogCloudService dialogCloudService, TalkableFinderOnLevel talkableFinder)
    {
        _player = player;
        _dialogManager = dialogManager;
        _dialogCloudService = dialogCloudService;
        _talkableFinder = talkableFinder;

        DeactivateDialogDelegate = dialogIndex => DeactivateDialog();
        _dialogManager.DeactivateDialog += DeactivateDialogDelegate;
    }

    public void ActivateDialog(string branchIndex)
    {
        _player.StartTalk();
        _dialogManager.ActivateDialog(branchIndex, _talkableFinder, _player.GetCancellationTokenOnDestroy());
    }

    public void DeactivateDialog()
    {
        _dialogCloudService.RemoveDialogCloud();
    }

    public void Dispose()
    {
        _dialogManager.DeactivateDialog -= DeactivateDialogDelegate;
    }
}
