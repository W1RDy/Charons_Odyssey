using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneManager : MonoBehaviour
{
    [SerializeField] private Image _image;
    public static LoadSceneManager Instance;
    public static bool _isNeedClose;
    private Action<float> _callback;
    private CancellationToken _token;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            _callback = value => _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, value);
            _token = this.GetCancellationTokenOnDestroy();
        }
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        if (_isNeedClose) _ = Instance.OpenCloseLoadingScreen(false);
    }

    private async UniTask OpenCloseLoadingScreen(bool _isOpen)
    {
        if (_token.IsCancellationRequested) return; 
        var (start, end) = _isOpen ? (0, 1) : (1, 0);
        if (_isOpen) _image.enabled = true;
        _isNeedClose = _isOpen;
        await SmoothChanger.SmoothChange(start, end, 2f, _callback, _token);
        if (!_isOpen && !_token.IsCancellationRequested) _image.enabled = false;
    }

    public void LoadScene(int _sceneIndex)
    {
        LoadScene(_sceneIndex, 0);
    }

    public void LoadScene(int _sceneIndex, int _delay)
    {
        AsyncLoading(_sceneIndex, _delay);
    }

    public void ReloadScene(int delay)
    {
        var _buildIndex = SceneManager.GetActiveScene().buildIndex;
        LoadScene(_buildIndex, delay);
    }

    public void LoadNextLevel()
    {
        var _buildIndex = SceneManager.GetActiveScene().buildIndex;
        LoadScene(_buildIndex + 1);
    }

    private async void AsyncLoading(int _sceneIndex, int _delay)
    {
        await UniTask.Delay(_delay, cancellationToken: _token);
        await OpenCloseLoadingScreen(true);
        if (_token.IsCancellationRequested) return;

        var _asyncOperation = SceneManager.LoadSceneAsync(_sceneIndex);
        _asyncOperation.allowSceneActivation = false;

        while (_asyncOperation.progress < 0.9f)
        {
            await UniTask.Yield();
            if (_token.IsCancellationRequested) return;
        }
        _asyncOperation.allowSceneActivation = true;
    }
}
