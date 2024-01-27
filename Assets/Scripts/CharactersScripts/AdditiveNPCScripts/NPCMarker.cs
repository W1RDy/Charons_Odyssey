using UnityEngine;

public class NPCMarker : MonoBehaviour 
{
    [SerializeField] private string _nameId;
    [SerializeField] private string _dialogId;
    [SerializeField] private Direction _direction;
    [SerializeField] private bool _isAvailable;
    public string NameId => _nameId;
    public string DialogId => _dialogId;
    public Direction Direction => _direction;
    public bool IsAvailable => _isAvailable;
}
