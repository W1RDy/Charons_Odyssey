using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class LoadSceneManager : MonoBehaviour
{
    [SerializeField] private Image _image;
    private Action<float> _callback;
    private CancellationToken _token;
    private DataController _dataController;
    private PauseService _pauseService;

    [Inject]
    private void Construct(DataController dataController, PauseService pauseService)
    {
        _dataController = dataController;
        _callback = value => _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, value);
        _token = this.GetCancellationTokenOnDestroy();
        _pauseService = pauseService;
    }

    private async UniTask OpenCloseLoadingScreen(bool isOpen)
    {
        if (_token.IsCancellationRequested) return;
        var (start, end) = isOpen ? (0, 1) : (1, 0);
        if (isOpen)
        {
            _image.enabled = true;
            _pauseService.SetUnpause();
            _pauseService.ChangePauseAvailable(false);
        }

        await SmoothChanger.SmoothChange(start, end, 2f, _callback, _token);
        if (!isOpen && !_token.IsCancellationRequested)
        {
            _image.enabled = false;
            _pauseService.ChangePauseAvailable(true);
        }

    }

    public void LoadScene(int sceneIndex)
    {
        LoadScene(sceneIndex, 0);
    }

    public void LoadScene(int sceneIndex, int delay)
    {
        if (_dataController.DataContainer.lastLocationIndex < sceneIndex)
        {
            _dataController.DataContainer.lastLocationIndex = sceneIndex;
            _dataController.SaveDatas();
        }
        AsyncLoading(sceneIndex, delay);
    }

    public void ReloadScene(int delay)
    {
        var buildIndex = SceneManager.GetActiveScene().buildIndex;
        LoadScene(buildIndex, delay);
    }

    public void LoadNextScene()
    {
        var buildIndex = SceneManager.GetActiveScene().buildIndex;
        LoadScene(buildIndex + 1);
    }

    private async void AsyncLoading(int sceneIndex, int delay)
    {
        await Delayer.Delay(delay, _token);
        await OpenCloseLoadingScreen(true);
        if (_token.IsCancellationRequested) return;

        var asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
        asyncOperation.allowSceneActivation = false;

        while (asyncOperation.progress < 0.9f)
        {
            await UniTask.Yield();
            if (_token.IsCancellationRequested) return;
        }
        asyncOperation.allowSceneActivation = true;
        await UniTask.WaitUntil(() => SceneManager.GetActiveScene().buildIndex == sceneIndex, cancellationToken: _token);
        await OpenCloseLoadingScreen(false);
    }
}
