using UnityEngine;

public class ArmorItemsService : MonoBehaviour
{
    [SerializeField] ArmorItem _shield;

    public Shield GetShield()
    {
        return _shield as Shield;
    }
}
