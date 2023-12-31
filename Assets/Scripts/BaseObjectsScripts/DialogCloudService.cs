using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogCloudService : MonoBehaviour
{
    [SerializeField] private DialogCloud _dialogCloudPrefab;
    [SerializeField] private float _cloudOffset;
    [SerializeField] private Transform _spawnParent;
    private DialogCloud _currentDialogCloud;

    public void SpawnDialogCloud(Vector3 pos)
    {
        if (_currentDialogCloud == null)
        {
            _currentDialogCloud = Instantiate(_dialogCloudPrefab, _spawnParent).GetComponent<DialogCloud>();
            SetSpawnPosition(pos);
        }
        else if (_currentDialogCloud.transform.position != pos)
        {
            SetSpawnPosition(pos);
        }
    }

    public void UpdateDialogCloud(string _message)
    {
        _currentDialogCloud.SetMessage(_message);
    }

    private void SetSpawnPosition(Vector2 pos)
    {
        _currentDialogCloud.transform.position = new Vector2(pos.x, pos.y + _cloudOffset);
    }

    public void RemoveDialogCloud()
    {
        if (_currentDialogCloud) Destroy(_currentDialogCloud.gameObject);
    }
}
