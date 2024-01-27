using UnityEngine;

public class NPCGroupMarker : MonoBehaviour
{
    [SerializeField] private string[] _npcsId;
    [SerializeField] private string _dialogId;
    [SerializeField] private bool _isAvailable = true;

    public string[] NpcsId => _npcsId;
    public string DialogId => _dialogId;
    public bool IsAvailable => _isAvailable;
}
