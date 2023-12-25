using UnityEngine;
using System.Collections.Generic;

public class Paddle : ColdWeapon
{
    public float RecliningForce => (_weaponData as PaddleData).RecliningForce;
}
