using UnityEngine;

public class MapLocation : MonoBehaviour
{
    [SerializeField] private string _name;

    public string Name => _name;
}