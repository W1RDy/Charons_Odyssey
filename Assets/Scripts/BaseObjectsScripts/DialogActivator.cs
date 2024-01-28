using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DialogActivator : IDisposable
{
    private Player _player;
    private DialogLifeController _dialogLifeController;
    private DialogCloudService _dialogCloudService;
    private Action<string> DeactivateDialogDelegate;

    [Inject]
    private void Construct(Player player, DialogLifeController dialogLifeController, DialogCloudService dialogCloudService)
    {
        _player = player;
        _dialogLifeController = dialogLifeController;
        _dialogCloudService = dialogCloudService;

        DeactivateDialogDelegate = dialogIndex => DeactivateDialog();
        _dialogLifeController.DeactivateDialog += DeactivateDialogDelegate;
    }

    public void ActivateDialog(string branchIndex)
    {
        _player.StartTalk();
        _dialogLifeController.StartDialog(branchIndex, _player.GetCancellationTokenOnDestroy());
    }

    public void DeactivateDialog()
    {
        _dialogCloudService.RemoveDialogCloud();
    }

    public void Dispose()
    {
        _dialogLifeController.DeactivateDialog -= DeactivateDialogDelegate;
    }
}
