using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelInfo[] _levels;
    public static LevelManager Instance;
    private Dictionary<int, LevelInfo> _levelsDictionary;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitDictionary();
        }
        else Destroy(gameObject);

        DontDestroyOnLoad(Instance);
    }

    private void InitDictionary()
    {
        _levelsDictionary = new Dictionary<int, LevelInfo>();

        foreach (var _level in _levels) _levelsDictionary[_level.index] = _level;
    }

    public LevelInfo GetLevel(int _index) => _levelsDictionary[_index];

    public LevelInfo GetNextLevel()
    {
        var _buildIndex = SceneManager.GetActiveScene().buildIndex;
        return GetLevel(_buildIndex + 1);
    }
}
