using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneManager : MonoBehaviour
{
    [SerializeField] private Image _image;
    public static LoadSceneManager Instance;
    public static bool _isNeedClose;
    Action<float> callback;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            callback = value => _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, value);
        }
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        if (_isNeedClose) Instance.OpenCloseLoadingScreen(false);
    }

    private void OpenCloseLoadingScreen(bool _isOpen)
    {
        var (start, end) = _isOpen ? (0, 1) : (1, 0);
        _isNeedClose = _isOpen;
        StartCoroutine(SmoothChanger<SmoothableFloat, float>.SmoothChange(new SmoothableFloat(start), end, 2f, callback));
    }

    public void LoadScene(int _sceneIndex)
    {
        StartCoroutine(AsyncLoading(_sceneIndex));
    }

    public void ReloadScene()
    {
        var _buildIndex = SceneManager.GetActiveScene().buildIndex;
        LoadScene(_buildIndex);
    }

    public void LoadNextLevel()
    {
        var _buildIndex = SceneManager.GetActiveScene().buildIndex;
        LoadScene(_buildIndex + 1);
    }

    private IEnumerator AsyncLoading(int _sceneIndex)
    {
        OpenCloseLoadingScreen(true);
        yield return new WaitForSeconds(2f);
        var _asyncOperation = SceneManager.LoadSceneAsync(_sceneIndex);
        _asyncOperation.allowSceneActivation = false;

        while (_asyncOperation.progress < 0.9f)
        {
            yield return null;
        }
        _asyncOperation.allowSceneActivation = true;
    }
}
