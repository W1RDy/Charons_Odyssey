using UnityEngine;

[CreateAssetMenu (fileName = "ShieldData", menuName = "WeaponsData/New ShieldData")]
public class ShieldData : ScriptableObject
{
    [SerializeField, Range(0, 100)] private float _absorbedDamage;
    
    public float AbsorbedDamage => _absorbedDamage;
}
