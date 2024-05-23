using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stay State", menuName = "Player's State/Stay State")]
public class PlayerStayState : PlayerState
{
    protected PlayerMove _playerMove;

    public override void Initialize(Player player, PauseService pauseService, AudioMaster audioMaster)
    {
        base.Initialize(player, pauseService, audioMaster);
        _playerMove = player.GetComponent<PlayerMove>();
    }
}
