using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "WeaponsData/New PaddleData")]
public class PaddleData : ColdWeaponData
{
    [SerializeField] private float _recliningForce;

    public float RecliningForce => _recliningForce;
}
