using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "New Data/New PlayerData")]
public class PlayerData : ScriptableObject
{
    [SerializeField] private float _hp = 100;
    [SerializeField] private float _speed;
    [SerializeField] private float _staminaValue = 100;
    [SerializeField] private float _stunningTime;

    public float Hp => _hp;
    public float Speed => _speed;
    public float StaminaValue => _staminaValue;
    public float StunningTime => _stunningTime;
}
