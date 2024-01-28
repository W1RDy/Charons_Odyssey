using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "Talk State", menuName = "Player's State/Talk State")]
public class PlayerTalkState : PlayerState
{
    private SpriteRenderer _spriteRenderer;
    private PlayerController _playerController;
    private DialogCloudService _dialogCloudService;
    private DialogLifeController _dialogLifeController;

    public virtual void Initialize(Player player, DialogLifeController dialogLifeController, DialogCloudService dialogCloudService)
    {
        base.Initialize(player);
        _spriteRenderer = player.transform.GetChild(0).GetComponent<SpriteRenderer>();
        _playerController = player.GetComponent<PlayerController>();
        _dialogCloudService = dialogCloudService;
        _dialogLifeController = dialogLifeController;
    }

    public override void Enter()
    {
        _playerController.IsControl = false;
        IsStateFinished = false;
    }

    public override void Update()
    {
        if (_dialogLifeController.DialogIsFinished()) IsStateFinished = true;
    }

    public void Talk(string message)
    {
        _dialogCloudService.SpawnDialogCloud(new Vector2(_player.transform.position.x, _player.transform.position.y + _spriteRenderer.sprite.bounds.size.y));
        _dialogCloudService.UpdateDialogCloud(message);
    }

    public override void Exit()
    {
        _playerController.IsControl = true;
    }
}
