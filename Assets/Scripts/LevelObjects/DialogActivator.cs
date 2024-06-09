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

    private GameStateController _gameStateController;

    [Inject]
    private void Construct(Player player, DialogLifeController dialogLifeController, DialogCloudService dialogCloudService, GameStateController gameStateController)
    {
        _player = player;
        _dialogLifeController = dialogLifeController;
        _dialogCloudService = dialogCloudService;

        DeactivateDialogDelegate = dialogIndex => DeactivateDialog();
        _dialogLifeController.DialogDeactivated += DeactivateDialogDelegate;

        _gameStateController = gameStateController;
    }

    public void ActivateDialog(string branchIndex)
    {
        _player.StartTalk();
        _dialogLifeController.StartDialog(branchIndex);

        _gameStateController.ActivateDialogState();
    }

    public void DeactivateDialog()
    {
        _dialogCloudService.RemoveDialogCloud();

        _gameStateController.ActivateResearchState();
    }

    public void Dispose()
    {
        _dialogLifeController.DialogDeactivated -= DeactivateDialogDelegate;
    }
}
