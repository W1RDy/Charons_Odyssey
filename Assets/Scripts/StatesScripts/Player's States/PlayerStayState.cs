using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "Stay State", menuName = "Player's State/Stay State")]
public class PlayerStayState : PlayerState
{
    protected PlayerMove _playerMove;

    public override void Initialize(Player player, IInstantiator instantiator, AudioMaster audioMaster)
    {
        base.Initialize(player, instantiator, audioMaster);
        _playerMove = player.GetComponent<PlayerMove>();
    }
}
