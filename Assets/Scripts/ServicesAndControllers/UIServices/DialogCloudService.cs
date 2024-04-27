using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DialogCloudService : MonoBehaviour, IService, IPause
{
    [SerializeField] private DialogCloud _dialogCloudPrefab;
    private DialogCloud _currentDialogCloud;

    [SerializeField] private Transform _spawnParent;
    [SerializeField] private float _cloudOffset;

    [SerializeField] private ScaleWithOffsetAnimation _appearAnimation;
    
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

            _appearAnimation.SetParameters(_currentDialogCloud.transform);

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
        _appearAnimation.Play();
    }

    public void RemoveDialogCloud()
    {
        if (_currentDialogCloud)
        {
            if (_appearAnimation.IsPlaying) _appearAnimation.Kill();
            Destroy(_currentDialogCloud.gameObject);
        }
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
