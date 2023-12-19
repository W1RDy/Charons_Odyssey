using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogCloudService : MonoBehaviour
{
    [SerializeField] private DialogCloud _dialogCloudPrefab;
    [SerializeField] private float _cloudOffset;
    private DialogCloud _currentDialogCloud;
    private Transform _spawnParent;

    private void Awake()
    {
        _spawnParent = GameObject.Find("WorldCanvas").transform;
    }

    public DialogCloud SpawnDialogCloud(ITalkable _talkable)
    {
        _currentDialogCloud = Instantiate(_dialogCloudPrefab, _spawnParent).GetComponent<DialogCloud>();
        SetSpawnPosition(_talkable);
        return _currentDialogCloud;
    }

    public void UpdateDialogCloud(string _message)
    {
        _currentDialogCloud.SetMessage(_message);
    }

    public void SetSpawnPosition(ITalkable _talkable)
    {
        var _topPoint = _talkable.GetTalkableTopPoint();
        _currentDialogCloud.transform.position = new Vector2(_topPoint.x, _topPoint.y + _cloudOffset);
    }

    public void RemoveDialogCloud()
    {
        if (_currentDialogCloud) Destroy(_currentDialogCloud.gameObject);
    }
}
