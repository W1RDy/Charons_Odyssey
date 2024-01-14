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
    private Action<string> DeactivateDialogDelegate;

    [Inject]
    private void Construct(Player player, DialogManager dialogManager, DialogCloudService dialogCloudService)
    {
        _player = player;
        _dialogManager = dialogManager;
        _dialogCloudService = dialogCloudService;

        DeactivateDialogDelegate = dialogIndex => DeactivateDialog();
        _dialogManager.DeactivateDialog += DeactivateDialogDelegate;
    }

    public void ActivateDialog(string branchIndex, ITalkable[] talkables)
    {
        _player.StartTalk();
        _dialogManager.ActivateDialog(branchIndex, talkables, _player.GetCancellationTokenOnDestroy());
    }

    public void ActivateDialog(string branchIndex, ITalkable talkable)
    {
        ActivateDialog(branchIndex, new ITalkable[] { talkable, _player });
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
