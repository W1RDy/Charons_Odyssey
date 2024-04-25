using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DialogCloudService : MonoBehaviour, IService, IPause
{
    [SerializeField] private DialogCloud _dialogCloudPrefab;
    [SerializeField] private float _cloudOffset;
    [SerializeField] private Transform _spawnParent;
    private DialogCloud _currentDialogCloud;
    private PauseService _pausedService;

    [Inject]
    private void Construct(PauseService pauseService)
    {
        _pausedService = pauseService;
        _pausedService.AddPauseObj(this);
    }

    public void InitializeService()
    {

    }

    public void SpawnDialogCloud(Vector3 pos)
    {
        if (_currentDialogCloud == null)
        {
            _currentDialogCloud = Instantiate(_dialogCloudPrefab, _spawnParent).GetComponent<DialogCloud>();
            _currentDialogCloud.transform.SetSiblingIndex(5);
            SetSpawnPosition(pos);
        }
        else if (_currentDialogCloud.transform.position != pos)
        {
            SetSpawnPosition(pos);
        }
    }

    public void UpdateDialogCloud(string message)
    {
        _currentDialogCloud.SetMessage(message);
    }

    private void SetSpawnPosition(Vector2 pos)
    {
        _currentDialogCloud.transform.position = new Vector2(pos.x, pos.y + _cloudOffset);
    }

    public void RemoveDialogCloud()
    {
        if (_currentDialogCloud) Destroy(_currentDialogCloud.gameObject);
    }

    public void Pause()
    {
        if (_currentDialogCloud) _currentDialogCloud.gameObject.SetActive(false);
    }

    public void Unpause()
    {
        if (_currentDialogCloud) _currentDialogCloud.gameObject.SetActive(true);
    }

    public void OnDestroy()
    {
        _pausedService.RemovePauseObj(this);
    }
}
